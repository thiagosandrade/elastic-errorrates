using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using ElasticErrorRates.Persistence.Repository;
using Nest;

namespace ElasticErrorRates.Persistence.Mappers
{
    public class LogElasticMappers : ILogElasticMappers
    {
        public ElasticResponse<LogSummary> MapElasticAggregateResults(ISearchResponse<Log> result)
        {
            IEnumerable<LogSummary> aggregatedResults = result.Aggregations.Terms("group_by_httpUrl").Buckets.Select(x =>
                {
                    if (x.DocCount != null)
                    {
                        var record = new LogSummary
                        {
                            HttpUrlCount = x.DocCount.Value,
                            HttpUrl = x.Key,
                            FirstOccurrence = string.Format("{0:yyyy/MM/dd hh:mm:ss}", Convert.ToDateTime(x.Min("first_occurrence").ValueAsString)),
                            LastOccurrence = string.Format("{0:yyyy/MM/dd hh:mm:ss}", Convert.ToDateTime(x.Max("last_occurrence").ValueAsString))
                        };

                        return record;
                    }

                    return null;
                }
            ).ToList();

            var totalRecords = aggregatedResults.Count();

            return new ElasticResponse<LogSummary>
            {
                TotalRecords = totalRecords,
                Records = aggregatedResults
            };
        }

        public ElasticResponse<Log> MapElasticResults(ISearchResponse<Log> result)
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
    }
}
