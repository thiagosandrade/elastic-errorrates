using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Criteria;
using ElasticErrorRates.Core.Criteria.Dashboard;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Elasticsearch.Net;
using Nest;

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

            _elasticContext.ElasticClient.ClearCache(defaultIndex);
        }

        private async Task<ISearchResponse<T>> BasicQuery(SearchDescriptor<T> queryCommand)
        {
            return await _elasticContext.ElasticClient.SearchAsync<T>(queryCommand.Index(defaultIndex).AllTypes());
        }

        public async Task<ElasticResponse<T>> Search(SearchCriteria criteria)
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

                                if (criteria.StartDate != DateTime.MinValue && criteria.EndDate != DateTime.MinValue)
                                {
                                    query &= fq.DateRange(
                                        descriptor => descriptor
                                            .Name("date_filter_start")
                                            .Field("startDate")
                                            .GreaterThanOrEquals(criteria.StartDate));

                                    query &= fq.DateRange(
                                        descriptor => descriptor
                                            .Name("date_filter_end")
                                            .Field("endDate")
                                            .LessThanOrEquals(criteria.EndDate));
                                }

                                return query;
                            }
                        )
                    )
                );

            var result = await BasicQuery(queryCommand);

            var response = _dashboardElasticMappers.MapElasticResults(result);

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
                        .Bool(bl =>
                                bl.Filter(fq => 
                                {
                                    QueryContainer query = null;

                                    query &= fq.Term(
                                        t => t.Field("countryId").Value(criteria.CountryId)
                                    );

                                    return query;
                                }
                            )
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
                   .Size(10);

            var result = await BasicQuery(queryCommand);

            var response = _dashboardElasticMappers.MapElasticAggregateResults(result, criteria.NumberOfResults);

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

        public async Task Delete(T log)
        {
            await _elasticContext.ElasticClient.DeleteAsync<T>(log, x => x.Index(defaultIndex));
        }

        public async Task Bulk(IEnumerable<DailyRate> records)
        {
            await _elasticContext.ElasticClient.BulkAsync(x => x.Index(defaultIndex).IndexMany(records));
        }
    }
}
