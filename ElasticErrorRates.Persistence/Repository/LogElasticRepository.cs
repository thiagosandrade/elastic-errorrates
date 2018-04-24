using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using ElasticErrorRates.Persistence.Context;
using Nest;

namespace ElasticErrorRates.Persistence.Repository
{
    public class LogElasticRepository<T> : ILogElasticRepository<T> where T : class
    {
        private readonly IElasticContext _elasticContext;
        private readonly ILogElasticMappers _logElasticMappers;
        private static readonly string defaultIndex = "errorlog";

        public LogElasticRepository(IElasticContext elasticContext, ILogElasticMappers logElasticMappers)
        {
            _elasticContext = elasticContext;
            _logElasticMappers = logElasticMappers;

            _elasticContext.SetupIndex<Log>(defaultIndex);

            _elasticContext.ElasticClient.ClearCache(defaultIndex);
        }

        public async Task<ElasticResponse<LogSummary>> SearchAggregate(int page, int pageSize)
        {
            var result = await _elasticContext.ElasticClient.SearchAsync<Log>(x => x.
                Index(defaultIndex)
                .AllTypes()
                .From(page * pageSize)
                .Size(pageSize)
                .Aggregations(ag => ag
                    .Terms("group_by_httpUrl", t => t.Field(f => f.HttpUrl.First().Suffix("keyword"))
                    )
                )
            );

            var response = _logElasticMappers.MapElasticAggregateResults(result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;

        }

        public async Task<ElasticResponse<Log>> Search(int page, int pageSize, string httpUrl)
        {
                var result = await _elasticContext.ElasticClient.SearchAsync<Log>(x => x.
                    Index(defaultIndex)
                    .AllTypes()
                    .From(page * pageSize)
                    .Size(pageSize)
                    .Query(q => q
                        .Bool(bl =>
                            bl.Filter(
                                fq =>
                                {
                                    QueryContainer query = null;

                                    if (httpUrl != "null")
                                    {
                                        query &= fq.QueryString(qs => qs
                                            .Fields(p => p
                                                .Field(f => f.HttpUrl)
                                            )
                                            .Query(httpUrl)
                                        );
                                    }

                                    return query;
                                }
                            )
                        )
                    )
                );

                var response = _logElasticMappers.MapElasticResults(result);

                if (!result.IsValid)
                {
                    throw new InvalidOperationException(result.DebugInformation);
                }

                return response;
            
        }

        public async Task<ElasticResponse<Log>> Find(string term, bool sort, bool match)
        {
                var result = await _elasticContext.ElasticClient.SearchAsync<Log>(x => x
                    .Index(defaultIndex)
                    .AllTypes()
                    .Query(q => q
                        .Bool(bl =>
                            bl.Filter(
                                fq =>
                                {
                                    QueryContainer query = null;

                                    if (term != "null")
                                    {
                                        if (!match)
                                        {
                                            query &= fq.Prefix(qs => qs
                                                .Field(f => f.Exception)
                                                .Value(term)
                                            );

                                            return query;
                                        }

                                        query &= fq.QueryString(qs => qs
                                            .Fields(p => p
                                                .Field(f => f.Exception)
                                            )
                                            .Query(term)
                                        );
                                    }

                                    return query;
                                }
                            )
                        )
                    )
                    .Sort(q =>
                    {
                        if (!sort)
                        {
                            return q
                                .Ascending(p => p.Id);
                        }

                        return q
                            .Field(p => p
                                .Field(f => f.DateTimeLogged)
                                .Order(SortOrder.Descending)
                            );
                    }
                    )
                    .From(0)
                    .Size(2000)
                    .Highlight(z => z
                        .Fields(y => y
                            .Field(p => p.Exception)
                                .PreTags("<b>")
                                .PostTags("</b>")
                            )
                        )
                    );

                var response = _logElasticMappers.MapElasticResults(result);

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

        public async Task Bulk(IEnumerable<Log> records)
        {
            await _elasticContext.ElasticClient.BulkAsync(x => x.Index(defaultIndex).IndexMany(records));
        }
    }
}
