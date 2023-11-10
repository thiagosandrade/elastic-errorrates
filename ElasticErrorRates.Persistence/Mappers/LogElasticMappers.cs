using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Nest;

namespace ElasticErrorRates.Persistence.Mappers
{
    public class LogElasticMappers<T> : ILogElasticMappers<T> where T : class
    {
        public async Task<ElasticResponse<T>> MapElasticResults(ISearchResponse<T> result)
        {
            IEnumerable<T> aggregatedResults = result.Aggregations.Terms("group_by_httpUrl").Buckets.Select(x =>
                {
                    if (x.DocCount != null)
                    {
                        var record = new LogSummary
                        {
                            HttpUrlCount = x.DocCount.Value,
                            HttpUrl = x.Key,
                            FirstOccurrence =
                                $"{Convert.ToDateTime(x.Min("first_occurrence").ValueAsString).ToUniversalTime():yyyy/MM/dd HH:mm:ss}",
                            LastOccurrence =
                                $"{Convert.ToDateTime(x.Max("last_occurrence").ValueAsString).ToUniversalTime():yyyy/MM/dd HH:mm:ss}"
                        };

                        return record;
                    }

                    return null;
                }
            ).Cast<T>().ToList();

            var totalRecords = aggregatedResults.Count();

            return await Task.Run(() =>
                new ElasticResponse<T>
                {
                    TotalRecords = totalRecords,
                    Records = aggregatedResults
                }
            );
        }

        public async Task<ElasticResponse<T>> MapElasticResults(ISearchResponse<T> result, string highlightTerm, bool updateData = false)
        {
            ISearchResponse<Log> convertedResult = (ISearchResponse<Log>) result;
            Random random = new();
            int days = 1;

            IEnumerable<T> records = convertedResult.Hits.Select(x =>
            {
                var log = new Log
                {
                    Id = !updateData ? x.Source.Id : random.Next(),
                    Level = x.Source.Level,
                    Message = x.Source.Message,
                    Source = x.Source.Source,
                    Exception = x.Source.Exception,
                    HttpUrl = x.Source.HttpUrl,
                    Highlight = x.Highlight.SelectMany(y => y.Value.ToList()).ToList().AsReadOnly(),
                    DateTimeLogged = updateData ? x.Source.DateTimeLogged.AddDays(days) : x.Source.DateTimeLogged
                };

                return log;

            }).Cast<T>().ToList();

            if (!updateData)
            {
                foreach (var log in records.OfType<Log>())
                {
                    string highlight = log.Highlight.FirstOrDefault();

                    if (!string.IsNullOrEmpty(highlight))
                    {
                        log.Exception = log.Exception.Replace(highlightTerm, highlight);
                    }
                }
            }

            return await Task.Run(() =>
                new ElasticResponse<T>
                {
                    TotalRecords = result.Total,
                    Records = records
                }
            );
        }

        public IEnumerable<T> UpdateDate(IEnumerable<T> listOfResultsToBeModified)
        {
            var listOfResultsConverted = (IEnumerable<Log>)listOfResultsToBeModified;

            IEnumerable<T> results = listOfResultsConverted.Select(x =>
            {
                var log = new Log
                {
                    Id = x.Id,
                    Exception = x.Exception,
                    HttpUrl = x.HttpUrl,
                    Level = x.Level,
                    Message = x.Message,
                    Source = x.Source,
                    DateTimeLogged = x.DateTimeLogged.AddDays(1)
                };

                return log;

            }).Cast<T>().ToList();

            var firstResult = results.OfType<Log>().OrderByDescending(x => x.DateTimeLogged).FirstOrDefault();
            if (firstResult != null 
                && (firstResult.DateTimeLogged.Day <= DateTime.Now.Day
                     && firstResult.DateTimeLogged.Month.Equals(DateTime.Now.Month) 
                     && firstResult.DateTimeLogged.Year.Equals(DateTime.Now.Year)))
            {
                results = results.Concat(UpdateDate(results));
            }

            return results;
        }
    }
}
