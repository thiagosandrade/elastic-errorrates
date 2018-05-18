using ElasticErrorRates.Core.Models;
using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticMappers<T> where T : class
    {
        ElasticResponse<T> MapElasticResults(ISearchResponse<T> result);
        ElasticResponse<T> MapElasticResults(ISearchResponse<T> result, string columnField,  string highlightTerm = "");
    }
}
