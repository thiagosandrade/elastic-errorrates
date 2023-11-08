using ElasticErrorRates.Core.Criteria.Dashboard;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Nest;
using ElasticErrorRates.Core.Criteria;

namespace ElasticErrorRates.Persistence.Repository
{
    public class DashboardElasticRepository<T> : IDashboardElasticRepository<T> where T : class
    {
        private readonly IElasticContext _elasticContext;
        private readonly IDashboardElasticMappers<T> _dashboardElasticMappers;
        private static readonly string defaultIndex = "dailyrates";

        public DashboardElasticRepository(IElasticContext elasticContext, IDashboardElasticMappers<T> dashboardElasticMappers)
        {
            _elasticContext = elasticContext;
            _dashboardElasticMappers = dashboardElasticMappers;

            _elasticContext.SetupIndex<DailyRate>(defaultIndex);

            //_elasticContext.ElasticClient.ClearCache(defaultIndex);
        }

        private async Task<ISearchResponse<T>> BasicQuery(SearchDescriptor<T> queryCommand)
        {
            return await _elasticContext.ElasticClient.SearchAsync<T>(queryCommand.Index(defaultIndex));
        }

        public async Task<ElasticResponse<T>> Search(DashboardSearchCriteria criteria)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                .Query(q => q
                    .Bool(bl =>
                        bl.Filter(
                            fq =>
                            {
                                QueryContainer query = null;

                                query &= fq.Term(
                                    t => t.Field("countryId").Value(criteria.CountryId)
                                );

                                if (criteria.StartDateTimeLogged != DateTime.MinValue && criteria.EndDateTimeLogged != DateTime.MinValue)
                                {
                                    query &= fq.DateRange(
                                        descriptor => descriptor
                                            .Name("date_filter_start")
                                            .Field("startDate")
                                            .GreaterThanOrEquals(criteria.StartDateTimeLogged));

                                    query &= fq.DateRange(
                                        descriptor => descriptor
                                            .Name("date_filter_end")
                                            .Field("endDate")
                                            .LessThanOrEquals(criteria.EndDateTimeLogged));
                                }

                                return query;
                            }
                        )
                    )
                );

            var result = await BasicQuery(queryCommand);

            var response = await _dashboardElasticMappers.MapElasticResults(result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;

        }

        public async Task<ElasticResponse<T>> SearchAggregate(GraphCriteria criteria)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                    .Query(q => q
                        .Bool(bl => bl
                            .Must(
                                fq =>
                                {
                                    QueryContainer query = null;

                                    query &= fq.Term(t => t
                                        .Field("countryId").Value(criteria.CountryId)
                                    );

                                    return query;
                                }
                            )
                            .Filter(
                                ft => new DateRangeQuery
                                {
                                    Field = "startDate",
                                    //GreaterThanOrEqualTo = criteria.StartDate,
                                    LessThanOrEqualTo = criteria.EndDateTimeLogged
                                })
                        )
                    )
                    .Aggregations(ag =>
                       {
                           AggregationContainerDescriptor<T> query = null;

                           query &= ag.DateHistogram("my_date_histogram", h => h
                               .Field("startDate")
                               .Interval((DateInterval)Int32.Parse(criteria.TypeAggregation))
                               .Order(HistogramOrder.KeyDescending)
                               .Aggregations( aa => aa
                                   .Sum("order-count",
                                       m => m.Field("orderCount"))
                                   .Sum("order-value",
                                       mm => mm.Field("orderValue"))
                                   .Sum("error-count",
                                       mm => mm.Field("errorCount"))
                                )
                           );

                           return query;
                       }
                   )
                   .Sort(q =>
                       {
                           return q
                               .Field(p => p
                                   .Field("startDate")
                                   .Order(SortOrder.Ascending)
                               );
                       }
                   )
                   .From(0)
                   .Size(criteria.NumberOfResults);

            var result = await BasicQuery(queryCommand);

            var response = await _dashboardElasticMappers.MapElasticAggregateResults(result, criteria.NumberOfResults);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;
        }

        public async Task Create(T log)
        {
            await _elasticContext.ElasticClient.IndexAsync<T>(log, x => x.Index(defaultIndex));
        }

        public async Task Delete(LogCriteria criteria)
        {
            var result = await _elasticContext.ElasticClient.DeleteByQueryAsync<T>(q => q
                .Index(defaultIndex)
                .Query(ft => new DateRangeQuery
                            {
                                Field = "endDate",
                                //GreaterThanOrEqualTo = criteria.EndDate,
                                LessThanOrEqualTo = criteria.EndDateTimeLogged
                            })
            );

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }
        }

        public async Task Bulk(IEnumerable<T> records)
        {
            var chunkedRecords = records.Chunk(1000);
            foreach (var chunkedRecord in chunkedRecords)
            {
                await _elasticContext.ElasticClient.BulkAsync(x => x.Index(defaultIndex).IndexMany(chunkedRecord));
            }
        }

        public async Task UpdateLogsToActualDate()
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>();
            queryCommand.Size(10000);

            await Task.Run(async () =>
            {
                var result = await BasicQuery(queryCommand);
                if (!result.IsValid)
                {
                    throw new InvalidOperationException(result.DebugInformation);
                }

                IEnumerable<T> listOfResultsToBeModified = await GetListWithUpdatedData(result);

                //await Delete(new DashboardSearchCriteria
                //{
                //    EndDateTimeLogged = DateTime.Now.AddDays(0)
                //});

                await Bulk(listOfResultsToBeModified);

            });
        }

        private async Task<IEnumerable<T>> GetListWithUpdatedData(ISearchResponse<T> result)
        {
            IEnumerable<T> listOfResultsToBeModified = new List<T>();

            var resultHits = result.Hits.Count;
            for (int i = 0; i < resultHits; i+=1000)
            {
                SearchDescriptor<T> queryCommandToRegisters = new SearchDescriptor<T>();
                queryCommandToRegisters.From(i).Size(1000);

                var response = await BasicQuery(queryCommandToRegisters);
                var registerToBeModified = await _dashboardElasticMappers.MapElasticResults(response, true);

                listOfResultsToBeModified = listOfResultsToBeModified.Concat(registerToBeModified.Records).ToList();
            }

            var checkDate = listOfResultsToBeModified.FirstOrDefault();
            if (checkDate != null)
            {
                var lastDateResource = (DateTime)checkDate.GetType().GetProperty("StartDate").GetValue(checkDate);
                if (!(lastDateResource.Day.Equals(DateTime.Now.Day - 1) && lastDateResource.Month.Equals(DateTime.Now.Month) && lastDateResource.Year.Equals(DateTime.Now.Year)))
                {
                    listOfResultsToBeModified = _dashboardElasticMappers.UpdateDate(listOfResultsToBeModified);
                }
            }

            return listOfResultsToBeModified;
        }
    }
}
