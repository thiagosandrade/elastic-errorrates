using ElasticErrorRates.Core.Models;
using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface IDashboardElasticMappers<T> where T : class
    {
        ElasticResponse<T> MapElasticResults(ISearchResponse<T> result);
        ElasticResponse<T> MapElasticAggregateResults(ISearchResponse<T> result, int numberOfResults);
    }
}
