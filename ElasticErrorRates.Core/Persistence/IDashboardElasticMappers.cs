using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;
using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface IDashboardElasticMappers<T> where T : class
    {
        Task<ElasticResponse<T>> MapElasticResults(ISearchResponse<T> result);
        Task<ElasticResponse<T>> MapElasticAggregateResults(ISearchResponse<T> result, int numberOfResults);
    }
}
