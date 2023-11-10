using ElasticErrorRates.Core.Criteria;
using ElasticErrorRates.Core.Criteria.Dashboard;
using ElasticErrorRates.Core.Criteria.Log;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Nest;

namespace ElasticErrorRates.Persistence.Repository
{
    public class LogElasticRepository<T> : ILogElasticRepository<T> where T : class
    {
        private readonly IElasticContext _elasticContext;
        private readonly ILogElasticMappers<T> _logElasticMappers;
        private static readonly string defaultIndex = "errorlog";

        public LogElasticRepository(IElasticContext elasticContext, ILogElasticMappers<T> logElasticMappers)
        {
            _elasticContext = elasticContext;
            _logElasticMappers = logElasticMappers;

            _elasticContext.SetupIndex<Log>(defaultIndex);

            //_elasticContext.ElasticClient.ClearCache(defaultIndex);
        }

        private async Task<ISearchResponse<T>> BasicQuery(SearchDescriptor<T> queryCommand)
        {
            return await Task.Run(() => _elasticContext.ElasticClient.SearchAsync<T>(queryCommand.Index(defaultIndex)));
        }

        private static async Task<Func<AggregationContainerDescriptor<T>, IAggregationContainer>> AggregateCommand()
        {
            return await Task.Run(() =>
             {
                 return new Func<AggregationContainerDescriptor<T>, IAggregationContainer>
                     (ag => ag
                         .Terms("group_by_httpUrl",
                             t => t.Field("httpUrl.keyword")
                                 .Aggregations(aa => aa
                                     .Min("first_occurrence",
                                         m => m.Field("dateTimeLogged"))
                                     .Max("last_occurrence",
                                         mm => mm.Field("dateTimeLogged"))
                                 )
                                 .Size(20)
                         )
                     );
             });
        }

        private static async Task<Func<SortDescriptor<T>, IPromise<IList<ISort>>>> SortCommand()
        {
            return await Task.Run(() => { 
                return new Func<SortDescriptor<T>, IPromise<IList<ISort>>>
                    (st => st
                            .Field(p => p
                                .Field("dateTimeLogged")
                                .Order(SortOrder.Descending)
                            )
                    );
            });
        }

        public async Task<ElasticResponse<T>> SearchLogsAggregate(SearchAgreggateCriteria criteria)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                .Query(q => q
                    .Bool(bl => bl
                        .Must(
                            ft => new DateRangeQuery
                            {
                                Field = "dateTimeLogged",
                                GreaterThanOrEqualTo = criteria.StartDateTimeLogged,
                                LessThanOrEqualTo = criteria.EndDateTimeLogged
                            })
                    )
                )
                .Size(0);

            queryCommand.Aggregations(LogElasticRepository<T>.AggregateCommand().Result);

            var result = await BasicQuery(queryCommand);

            var response = await _logElasticMappers.MapElasticResults(result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;

        }

        public async Task<ElasticResponse<T>> SearchLogsAggregateByCountryId(GraphCriteria searchCriteria)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                .Query(q => q
                    .Bool(bl => bl
                        .Must(
                            fq =>
                            {
                                QueryContainer query = null;

                                query &= fq.Term(t => t
                                    .Field("countryId").Value(searchCriteria.CountryId)
                                );

                                return query;
                            }
                        )
                        .Filter(
                            ft => new DateRangeQuery
                            {
                                Field = "dateTimeLogged",
                                GreaterThanOrEqualTo = searchCriteria.StartDateTimeLogged,
                                LessThanOrEqualTo = searchCriteria.EndDateTimeLogged
                            })
                    )
                );

            queryCommand.Aggregations(LogElasticRepository<T>.AggregateCommand().Result);

            var result = await BasicQuery(queryCommand);

            var response = await _logElasticMappers.MapElasticResults(result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;
        }

        public async Task<ElasticResponse<T>> SearchLogsDetailed(LogSearchCriteria criteria)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                .Query(q => q
                    .Bool(bl =>
                        bl.Must(
                            fq =>
                            {
                                QueryContainer query = null;

                                if (criteria.HttpUrl != "null")
                                {
                                    query &= fq.Term(
                                            t => t.Field("httpUrl.keyword").Value(criteria.HttpUrl)
                                    );
                                }

                                return query;
                            }
                        )
                         .Filter(
                            ft => new DateRangeQuery
                            {
                                Field = "dateTimeLogged",
                                GreaterThanOrEqualTo = criteria.StartDateTimeLogged,
                                LessThanOrEqualTo = criteria.EndDateTimeLogged
                            })
                    )
                )
                .Sort(LogElasticRepository<T>.SortCommand().Result)
                .From(criteria.Page * criteria.PageSize)
                .Size(criteria.PageSize);

            var result = await BasicQuery(queryCommand);

            var response = await _logElasticMappers.MapElasticResults(result, "exception");

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;
        }

        public async Task<ElasticResponse<T>> Find(LogSearchCriteria criteria)
        {
            var result = await FindLog(criteria);

            switch (criteria.ColumnField)
            {
                case "exception":
                    
                    return await _logElasticMappers.MapElasticResults(result, criteria.Term);

                case "httpUrl":

                    return await _logElasticMappers.MapElasticResults(result);
            }

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return default!;
        }

        private async Task<ISearchResponse<T>> FindLog(LogSearchCriteria criteria)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                .Query(q => q
                    .Bool(bl => bl
                        .Must(
                            fq =>
                            {
                                QueryContainer query = null;

                                query &= fq.Term(t => t
                                    .Field(criteria.ColumnField)
                                    .Value(criteria.Term.ToLower())
                                );

                                return query;
                            }
                        )
                        .Filter(
                            fq =>
                            {
                                QueryContainer query = null;

                                query &= fq.Term(
                                    t => t.Field($"httpUrl.keyword").Value(criteria.HttpUrl)
                                );

                                if (criteria.StartDateTimeLogged != DateTime.MinValue && criteria.EndDateTimeLogged != DateTime.MinValue)
                                {
                                    query &= fq.DateRange(
                                        descriptor => descriptor
                                            .Name("date_filter_start")
                                            .Field("dateTimeLogged")
                                            .GreaterThanOrEquals(criteria.StartDateTimeLogged));

                                    query &= fq.DateRange(
                                        descriptor => descriptor
                                            .Name("date_filter_end")
                                            .Field("dateTimeLogged")
                                            .LessThanOrEquals(criteria.EndDateTimeLogged));
                                }

                                return query;
                            }
                        )
                    )
                )
                .Sort(LogElasticRepository<T>.SortCommand().Result)
                .From(0)
                .Size(10)
                .Highlight(z => z
                    .Fields(y => y
                        .Field(criteria.ColumnField)
                        .PreTags("<span class=\"highlight\">")
                        .PostTags("</span>")
                    )
                .NumberOfFragments(10)
                .FragmentSize(1)
                .Order(HighlighterOrder.Score)
                );
                

            //queryCommand.Aggregations(LogElasticRepository<T>.AggregateCommand().Result);

            return await BasicQuery(queryCommand);
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
                    Field = "dateTimeLogged",
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

        public async Task<long> GetLogsQuantity(LogQuantityCriteria criteria)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                .Query(q => q
                    .Bool(bl =>
                        bl.Filter(
                            ft => new DateRangeQuery
                            {
                                Field = "dateTimeLogged",
                                GreaterThanOrEqualTo = criteria.StartDateTimeLogged,
                                LessThanOrEqualTo = criteria.EndDateTimeLogged
                            })
                    )
                );

            return await Task.Run(async () =>
            {
                try
                {
                    var result = await BasicQuery(queryCommand);

                    if (!result.IsValid)
                    {
                        throw new InvalidOperationException(result.DebugInformation);
                    }

                    return result.Total;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            });
        }

        public async Task UpdateLogsToActualDate()
        {
            SearchDescriptor<T> queryCommand = new();
            queryCommand.Size(10000);

            await Task.Run(async () =>
            {
                var result = await BasicQuery(queryCommand);
                if (!result.IsValid)
                {
                    throw new InvalidOperationException(result.DebugInformation);
                }

                IEnumerable<T> listOfResultsToBeModified = await GetListWithUpdatedData(result);

                //await Delete(new LogSearchCriteria
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
            for (int i = 0; i < resultHits; i += 1000)
            {
                SearchDescriptor<T> queryCommandToRegisters = new();
                queryCommandToRegisters.From(i).Size(1000);

                var response = await BasicQuery(queryCommandToRegisters);
                var registerToBeModified = await _logElasticMappers.MapElasticResults(response, "", true);

                listOfResultsToBeModified = listOfResultsToBeModified.Concat(registerToBeModified.Records).ToList();
            }

            var checkDate = listOfResultsToBeModified.FirstOrDefault();
            if (checkDate != null)
            {
                var lastDateResource = (DateTime)checkDate.GetType().GetProperty("DateTimeLogged").GetValue(checkDate);
                if (!(lastDateResource.Day.Equals(DateTime.Now.Day - 1) && lastDateResource.Month.Equals(DateTime.Now.Month) && lastDateResource.Year.Equals(DateTime.Now.Year)))
                {
                    listOfResultsToBeModified = _logElasticMappers.UpdateDate(listOfResultsToBeModified);
                }
            }

            return listOfResultsToBeModified;
        }
    }
}
