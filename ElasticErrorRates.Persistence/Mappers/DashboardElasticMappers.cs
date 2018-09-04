using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Nest;

namespace ElasticErrorRates.Persistence.Mappers
{
    public class DashboardElasticMappers<T> : IDashboardElasticMappers<T> where T : class
    {
        public async Task<ElasticResponse<T>> MapElasticResults(ISearchResponse<T> result)
        {
            ISearchResponse<DailyRate> convertedResult = (ISearchResponse<DailyRate>)result;

            IEnumerable<T> records = convertedResult.Hits.Select(x =>
            {
                var log = new DailyRate
                {
                    Id = x.Source.Id,
                    CountryId = x.Source.CountryId,
                    StartDate = x.Source.StartDate,
                    EndDate = x.Source.EndDate,
                    ErrorCount = x.Source.ErrorCount,
                    OrderCount = x.Source.OrderCount,
                    OrderValue = x.Source.OrderValue,
                    ErrorPercentage = Math.Round((float)x.Source.ErrorCount / x.Source.OrderCount * 100,2)
                };

                return log;

            }).Cast<T>().ToList();

            return await Task.Run(() =>
                new ElasticResponse<T>
                {
                    TotalRecords = result.Total,
                    Records = records
                }
            );
        }

        public async Task<ElasticResponse<T>> MapElasticAggregateResults(ISearchResponse<T> result, int numberOfResults)
        {
            IEnumerable<T> aggregatedResults = result.Aggregations.DateHistogram("my_date_histogram").Buckets.Select(x =>
                {
                    if (x.DocCount != null)
                    {
                        var orderCount = ((ValueAggregate) x.Values.Skip(0).Take(1).FirstOrDefault())?.Value;
                        var errorCount = ((ValueAggregate) x.Values.Skip(1).Take(1).FirstOrDefault())?.Value;
                        var orderValue = ((ValueAggregate) x.Values.Skip(2).Take(1).FirstOrDefault())?.Value;

                        var record = new ErrorRate
                        {
                            Date = string.Format("{0:yyyy/MM/dd}", DateTime.Parse(x.KeyAsString)),
                            OrderCount = orderCount.ToString(),
                            ErrorCount = errorCount.ToString(),
                            OrderValue = orderValue.ToString(),
                            ErrorPercentage = 
                                    Math.Round(errorCount != null && orderCount != null ? 
                                        (double) (errorCount / orderCount * 100) : 0 
                                    ,2).ToString(CultureInfo.InvariantCulture)
                        };

                        return record;
                    }

                    return null;
                }
            ).Cast<T>().ToList();

            return await Task.Run(() => 
                new ElasticResponse<T>
                {
                    TotalRecords = aggregatedResults.Count(),
                    Records = aggregatedResults.Take(numberOfResults)
                }
            );
        }
    }


}
