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
        private static readonly string defaultIndex = "errorlog";

        public LogElasticRepository(IElasticContext elasticContext)
        {
            _elasticContext = elasticContext;

            _elasticContext.SetupIndex<Log>(defaultIndex);

        }

        public async Task<ElasticResponse<Log>> Search()
        {
                var result = await _elasticContext.ElasticClient.SearchAsync<Log>(x => x.
                    Index(defaultIndex)
                    .AllTypes()
                    .From(0)
                    .Size(2000)
                );

                var response = MapElasticResults(result);

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

                var response = MapElasticResults(result);

                if (!result.IsValid)
                {
                    throw new InvalidOperationException(result.DebugInformation);
                }

                return response;
        }

        private ElasticResponse<Log> MapElasticResults(ISearchResponse<Log> result)
        {
            var records = result.Hits.Select(x =>
            {
                var log = new Log
                {
                    Id = x.Source.Id,
                    Level = x.Source.Level,
                    Message = x.Source.Message,
                    Source = x.Source.Source,
                    Exception = x.Source.Exception,
                    HttpUrl = x.Source.HttpUrl,
                    Highlight = x.Highlights.Values.FirstOrDefault()?.Highlights.FirstOrDefault()?.ToString(),
                    DateTimeLogged = x.Source.DateTimeLogged
                };

                return log;

            }).ToList();

            var totalRecords = result.Hits.Count;

            return new ElasticResponse<Log>
            {
                TotalRecords = totalRecords,
                Records = records
            };
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
            await _elasticContext.ElasticClient.BulkAsync(x => x.IndexMany(records));
        }
    }
}
