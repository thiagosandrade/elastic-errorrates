using System;
using System.Collections.Generic;
using System.Text;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Persistence.Repository;
using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticMappers
    {
        ElasticResponse<LogSummary> MapElasticAggregateResults(ISearchResponse<Log> result);
        ElasticResponse<Log> MapElasticResults(ISearchResponse<Log> result);
    }
}
