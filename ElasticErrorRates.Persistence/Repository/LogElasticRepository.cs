using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
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

        public async Task<ElasticResponse<LogSummary>> SearchAggregate()
        {
            var result = await _elasticContext.ElasticClient.SearchAsync<Log>(x => x.
                Index(defaultIndex)
                .AllTypes()
                .Aggregations(ag =>
                {
                    ag.Terms("group_by_httpUrl",
                        t => t.Field(f => f.HttpUrl.First().Suffix("keyword"))
                            .Aggregations(aa => aa
                                .Min("first_occurrence",
                                    m => m.Field(f => f.DateTimeLogged))
                                .Max("last_occurrence",
                                    mm => mm.Field(ff => ff.DateTimeLogged))
                            ).Size(Int32.MaxValue)
                    );
                    return ag;
                })
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
                .Query(q => q
                    .Bool(bl =>
                        bl.Filter(
                            fq =>
                            {
                                QueryContainer query = null;

                                if (httpUrl != "null")
                                {
                                    query &= fq.Term(
                                            t => t.Field(f => f.HttpUrl.First().Suffix("keyword")
                                        ).Value(httpUrl)
                                    );
                                }

                                return query;
                            }
                        )
                    )
                )
                .From(page * pageSize)
                .Size(pageSize)
            );

            var response = _logElasticMappers.MapElasticResults(result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;

        }

        public async Task<ElasticResponse<Log>> Find(string httpUrl, string term)
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
                                    query &= fq.Match(qs => qs
                                        .Field(ff => ff.Exception)
                                        .Query(term)
                                        .MinimumShouldMatch("80%")
                                    );

                                    
                                }

                                if (httpUrl != "null")
                                {
                                    query &= fq.Term(
                                        t => t.Field(f => f.HttpUrl.First().Suffix("keyword")
                                        ).Value(httpUrl)
                                    );
                                }

                                return query;
                            }
                        )
                    )
                )
                .Sort(q =>
                {
                    return q
                        .Field(p => p
                            .Field(f => f.DateTimeLogged)
                            .Order(SortOrder.Descending)
                        );
                }
                )
                .From(0)
                .Size(10)
                .Highlight(z => z
                    .Fields(y => y
                        .Field(p => p.Exception)
                            .PreTags("<b>")
                            .PostTags("</b>")
                        )
                        .NumberOfFragments(10)
                        .FragmentSize(1)
                        .Order(HighlighterOrder.Score)
                    )
                );

            var response = _logElasticMappers.MapElasticResults(result, term);

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
