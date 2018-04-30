using ElasticErrorRates.Core.Models;
using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticMappers
    {
        ElasticResponse<LogSummary> MapElasticAggregateResults(ISearchResponse<Log> result);
        ElasticResponse<Log> MapElasticResults(ISearchResponse<Log> result);
    }
}
