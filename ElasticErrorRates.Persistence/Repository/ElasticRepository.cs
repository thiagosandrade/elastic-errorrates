using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Nest;

namespace ElasticErrorRates.Persistence.Repository
{
    public class ElasticRepository<T> : IElasticRepository<T> where T : class
    { 
        private static readonly ConnectionSettings ConnSettings = new ConnectionSettings(new Uri("http://pc092nel.hitachiconsulting.net:9200/"));
        private static readonly ElasticClient ElasticClient = new ElasticClient(ConnSettings);
        private static readonly string defaultIndex = "default";

        public ElasticRepository()
        {
            if (!ElasticClient.IndexExists(defaultIndex).Exists)
            {
                ElasticClient.CreateIndex(defaultIndex, i => i
                    .Mappings(m => m
                        .Map<Shakespeare>(ms => ms.AutoMap())
                    )
                );
            }
        }

        public async Task<ElasticResponse<Shakespeare>> Search()
        {
                var result = await ElasticClient.SearchAsync<Shakespeare>(x => x.
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

        public async Task<ElasticResponse<Shakespeare>> Find(string term, bool sort, bool match)
        {
                var result = await ElasticClient.SearchAsync<Shakespeare>(x => x
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
                                                .Field(f => f.TextEntry)
                                                .Value(term)
                                            );

                                            return query;
                                        }

                                        query &= fq.QueryString(qs => qs
                                            .Fields(p => p
                                                .Field(f => f.TextEntry)
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
                                .Field(f => f.Name)
                                .Order(SortOrder.Ascending)
                            );
                    }
                    )
                    .From(0)
                    .Size(2000)
                    .Highlight(z => z
                        .Fields(y => y
                            .Field(p => p.TextEntry)
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

        private ElasticResponse<Shakespeare> MapElasticResults(ISearchResponse<Shakespeare> result)
        {
            var records = result.Hits.Select(x =>
            {
                var shakespeare = new Shakespeare
                {
                    LineNumber = x.Source.LineNumber,
                    DocumentId = Int32.Parse(x.Id),
                    Name = x.Source.Name,
                    Id = x.Source.Id,
                    Speaker = x.Source.Speaker,
                    SpeechNumber = x.Source.SpeechNumber,
                    Highlight = x.Highlights.Values.FirstOrDefault()?.Highlights.FirstOrDefault()?.ToString(),
                    TextEntry = x.Source.TextEntry
                };

                return shakespeare;

            }).ToList();

            var totalRecords = result.Hits.Count;

            return new ElasticResponse<Shakespeare>
            {
                TotalRecords = totalRecords,
                Records = records
            };
        }


        public async Task Create(Shakespeare product)
        {
            await ElasticClient.IndexAsync<Shakespeare>(product, x => x.Index(defaultIndex));
        }

        public async Task Delete(Shakespeare product)
        {
            await ElasticClient.DeleteAsync<Shakespeare>(product, x => x.Index(defaultIndex));
        }

        
    }
}
