using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Criteria;
using ElasticErrorRates.Core.Criteria.Dashboard;
using ElasticErrorRates.Core.Criteria.Log;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using ElasticErrorRates.Persistence.Utils;
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
            return await Task.Run(() => _elasticContext.ElasticClient.SearchAsync<T>(queryCommand.Index(defaultIndex).AllTypes()));
        }

        public async Task<Func<AggregationContainerDescriptor<T>, IAggregationContainer>> AggregateCommand()
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

        public async Task<ElasticResponse<T>> SearchAggregate(SearchAgreggateCriteria criteria)
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

            queryCommand.Aggregations(AggregateCommand().Result);

            var result = await BasicQuery(queryCommand);

            var response = _logElasticMappers.MapElasticResults(result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;

        }

        public async Task<ElasticResponse<T>> SearchAggregateByCountryId(GraphCriteria searchCriteria)
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

                                query &= fq.Term(t => t
                                    .Field("level").Value("error")
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

            queryCommand.Aggregations(AggregateCommand().Result);

            var result = await BasicQuery(queryCommand);

            var response = _logElasticMappers.MapElasticResults(result);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return response;
        }

        public async Task<ElasticResponse<T>> Search(LogSearchCriteria criteria)
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

        public async Task<ElasticResponse<T>> Find(LogSearchCriteria criteria)
        {
            var result = await FindLog(criteria);

            switch (criteria.ColumnField)
            {
                case "exception":
                    
                    return _logElasticMappers.MapElasticResults(result, criteria.Term);

                case "httpUrl":

                    return _logElasticMappers.MapElasticResults(result);
            }

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.DebugInformation);
            }

            return null;
        }

        private async Task<ISearchResponse<T>> FindLog(LogSearchCriteria searchCriteria)
        {
            SearchDescriptor<T> queryCommand = new SearchDescriptor<T>()
                .Query(q => q
                    .Bool(bl => bl
                        .Must(
                            fq =>
                            {
                                QueryContainer query = null;

                                //if (searchCriteria.Term != "null")
                                //{
                                //    query &= fq.Match(qs => qs
                                //        .Field(searchCriteria.ColumnField)
                                //        .Query(searchCriteria.Term)
                                //        .MinimumShouldMatch("80%")
                                //    );
                                //}

                                query &= fq.Term(t => t
                                    .Field("httpUrl")
                                    .Value(searchCriteria.Term)
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
                            }
                        )
                    )
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
                        .Field(searchCriteria.ColumnField)
                        .PreTags("<u>")
                        .PostTags("</u>")
                    )
                    .NumberOfFragments(10)
                    .FragmentSize(1)
                    .Order(HighlighterOrder.Score)
                );
                

            queryCommand.Aggregations(AggregateCommand().Result);

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
            var chunckedRecords = records.Chunk(1000);
            foreach (var chunckedRecord in chunckedRecords)
            {
                await _elasticContext.ElasticClient.BulkAsync(x => x.Index(defaultIndex).IndexMany(chunckedRecord));       
            }
         
        }
    }
}
