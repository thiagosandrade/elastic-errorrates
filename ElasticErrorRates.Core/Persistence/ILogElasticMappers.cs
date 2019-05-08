using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;
using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticMappers<T> where T : class
    {
        Task<ElasticResponse<T>> MapElasticResults(ISearchResponse<T> result);
        Task<ElasticResponse<T>> MapElasticResults(ISearchResponse<T> result, string highlightTerm, bool updateData = false);
        IEnumerable<T> UpdateDate(IEnumerable<T> listOfResultsToBeModified);
    }
}
