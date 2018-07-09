using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Nest;

namespace ElasticErrorRates.Persistence.Mappers
{
    public class DashboardElasticMappers<T> : IDashboardElasticMappers<T> where T : class
    {
        public ElasticResponse<T> MapElasticResults(ISearchResponse<T> result)
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
                    ErrorPercentage = Math.Round((((float)x.Source.ErrorCount / x.Source.OrderCount) * 100),2)
                };

                return log;

            }).Cast<T>().ToList();

            return new ElasticResponse<T>
            {
                TotalRecords = result.Total,
                Records = records
            };
        }
    }


}
