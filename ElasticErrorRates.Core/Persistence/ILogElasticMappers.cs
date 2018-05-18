using ElasticErrorRates.Core.Models;
using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticMappers<T> where T : class
    {
        ElasticResponse<T> MapElasticAggregateResults(ISearchResponse<T> result);
        ElasticResponse<T> MapElasticResults(string columnField, ISearchResponse<T> result, string highlightTerm = "");
    }
}
