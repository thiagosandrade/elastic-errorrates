using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Criteria.Log;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Elasticsearch.Net;
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

            _elasticContext.ElasticClient.ClearCache(defaultIndex);
        }

        private async Task<ISearchResponse<T>> BasicQuery(SearchDescriptor<T> queryCommand)
        {
            return await _elasticContext.ElasticClient.SearchAsync<T>(queryCommand.Index(defaultIndex).AllTypes());
        }

        public async Task<ElasticResponse<T>> SearchAggregate()
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                .Aggregations(ag => ag
                    .Terms("group_by_httpUrl",
                        t => t.Field("httpUrl.keyword")
                            .Aggregations(aa => aa
                                .Min("first_occurrence",
                                    m => m.Field("dateTimeLogged"))
                                .Max("last_occurrence",
                                    mm => mm.Field("dateTimeLogged"))
                            )
                            .Size(int.MaxValue)
                        )
                );


            var result = await BasicQuery(queryCommand);

            var response = _logElasticMappers.MapElasticResults(result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;

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

                                if (criteria.HttpUrl != "null")
                                {
                                    query &= fq.Term(
                                            t => t.Field("httpUrl.keyword").Value(criteria.HttpUrl)
                                    );
                                }

                                return query;
                            }
                        )
                    )
                )
                .From(criteria.Page * criteria.PageSize)
                .Size(criteria.PageSize);

            var result = await BasicQuery(queryCommand);

            var response = _logElasticMappers.MapElasticResults(result, "exception");

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;

        }

        public async Task<ElasticResponse<T>> Find(FindCriteria criteria)
        {
            var result = await FindLog(criteria.ColumnField, criteria.HttpUrl, criteria.Term);

            switch (criteria.ColumnField)
            {
                case "exception":
                    
                    return _logElasticMappers.MapElasticResults(result, criteria.ColumnField, criteria.Term);

                case "httpUrl":

                    return _logElasticMappers.MapElasticResults(result);
            }

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return null;
        }

        private async Task<ISearchResponse<T>> FindLog(string columnField, string httpUrl, string term)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                    .Query(q => q
                        .Bool(bl =>
                            bl.Filter(
                                fq =>
                                {
                                    QueryContainer query = null;

                                    if (term != "null")
                                    {
                                        query &= fq.Match(qs => qs
                                            .Field(columnField)
                                            .Query(term)
                                            .MinimumShouldMatch("80%")
                                        );
                                    }

                                    if (httpUrl != "null")
                                    {
                                        query &= fq.Term(t => t
                                            .Field("httpUrl.keyword").Value(httpUrl)
                                        );
                                    }

                                    return query;
                                }
                            )
                        )
                    )
                    .Aggregations(ag =>
                        {
                            AggregationContainerDescriptor<T> query = null;

                            if (columnField.Equals("httpUrl"))
                            {
                                query &= ag.Terms("group_by_httpUrl",
                                        t => t.Field("httpUrl.keyword")
                                            .Aggregations(aa => aa
                                                .Min("first_occurrence",
                                                    m => m.Field("dateTimeLogged"))
                                                .Max("last_occurrence",
                                                    mm => mm.Field("dateTimeLogged"))
                                            )
                                            .Size(int.MaxValue)
                                    );
                            }

                            return query;
                        }
                    )
                    .Sort(q =>
                        {
                            return q
                                .Field(p => p
                                    .Field("dateTimeLogged")
                                    .Order(SortOrder.Descending)
                                );
                        }
                    )
                    .From(0)
                    .Size(10)
                    .Highlight(z => z
                        .Fields(y => y
                            .Field(columnField)
                            .PreTags("<b>")
                            .PostTags("</b>")
                        )
                        .NumberOfFragments(10)
                        .FragmentSize(1)
                        .Order(HighlighterOrder.Score)
                    );

            return await BasicQuery(queryCommand);
        }

        public async Task Create(T log)
        {
            await _elasticContext.ElasticClient.IndexAsync<T>(log, x => x.Index(defaultIndex));
        }

        public async Task Delete(T log)
        {
            await _elasticContext.ElasticClient.DeleteAsync<T>(log, x => x.Index(defaultIndex));
        }

        public async Task Bulk(IEnumerable<Log> records)
        {
            await _elasticContext.ElasticClient.BulkAsync(x => x.Index(defaultIndex).IndexMany(records));
        }
    }
}
