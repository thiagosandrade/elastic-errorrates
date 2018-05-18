using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var response = _logElasticMappers.MapElasticAggregateResults(result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;

        }

        public async Task<ElasticResponse<T>> Search(int page, int pageSize, string httpUrl)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                .Query(q => q
                    .Bool(bl =>
                        bl.Filter(
                            fq =>
                            {
                                QueryContainer query = null;

                                if (httpUrl != "null")
                                {
                                    query &= fq.Term(
                                            t => t.Field("httpUrl.keyword").Value(httpUrl)
                                    );
                                }

                                return query;
                            }
                        )
                    )
                )
                .From(page * pageSize)
                .Size(pageSize);

            var result = await BasicQuery(queryCommand);

            var response = _logElasticMappers.MapElasticResults("exception", result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;

        }

        public async Task<ElasticResponse<T>> Find(string columnField, string httpUrl, string term)
        {
            ISearchResponse<T> result = new SearchResponse<T>();

            switch (columnField)
            {
                case "exception":

                    result = await FindLog(columnField, httpUrl, term);
                    return _logElasticMappers.MapElasticResults(columnField, result, term);

                case "httpUrl":

                    result = await FindLog(columnField, httpUrl, term);
                    return _logElasticMappers.MapElasticAggregateResults(result);
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

                            if (columnField.Equals("httpUrl") && !string.IsNullOrWhiteSpace(term))
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
