using ElasticErrorRates.Core.Models;
using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface IDashboardElasticMappers<T> where T : class
    {
        Task<ElasticResponse<T>> MapElasticResults(ISearchResponse<T> result, bool updateData = false);
        Task<ElasticResponse<T>> MapElasticAggregateResults(ISearchResponse<T> result, int numberOfResults);
        IEnumerable<T> UpdateDate(IEnumerable<T> listOfResultsToBeModified);
    }
}
