using System;
using System.Collections.Generic;
using System.Linq;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Nest;

namespace ElasticErrorRates.Persistence.Mappers
{
    public class LogElasticMappers<T> : ILogElasticMappers<T> where T : class
    {
        public ElasticResponse<T> MapElasticResults(ISearchResponse<T> result)
        {
            IEnumerable<T> aggregatedResults = result.Aggregations.Terms("group_by_httpUrl").Buckets.Select(x =>
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
            ).Cast<T>().ToList();

            var totalRecords = aggregatedResults.Count();

            return new ElasticResponse<T>
            {
                TotalRecords = totalRecords,
                Records = aggregatedResults
            };
        }

        public ElasticResponse<T> MapElasticResults(ISearchResponse<T> result, string columnField, string highlightTerm = "")
        {
            ISearchResponse<Log> convertedResult = (ISearchResponse<Log>) result;

            IEnumerable<T> records = convertedResult.Hits.Select(x =>
            {
                var log = new Log
                {
                    Id = x.Source.Id,
                    Level = x.Source.Level,
                    Message = x.Source.Message,
                    Source = x.Source.Source,
                    Exception = x.Source.Exception,
                    HttpUrl = x.Source.HttpUrl,
                    Highlight = x.Highlights.SelectMany(y => y.Value.Highlights.ToList()).ToList().AsReadOnly(),
                    DateTimeLogged = x.Source.DateTimeLogged
                };

                return log;

            }).Cast<T>().ToList();

            foreach (var log in records.OfType<Log>())
            {
                string highlight = log.Highlight.FirstOrDefault();

                if (!string.IsNullOrEmpty(highlight))
                {
                    log.Exception = log.Exception.Replace(highlightTerm, highlight.Substring(highlight.ToLower().IndexOf(highlightTerm.ToLower(), StringComparison.Ordinal) - 3,
                        highlight.IndexOf(highlightTerm.ToLower(), StringComparison.Ordinal) + highlightTerm.Length + 8));
                }
            }

            return new ElasticResponse<T>
            {
                TotalRecords = result.Total,
                Records = records
            };
        }
    }


}
