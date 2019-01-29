using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Criteria;
using ElasticErrorRates.Core.Criteria.Dashboard;
using ElasticErrorRates.Core.Criteria.Log;
using ElasticErrorRates.Core.Models;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticRepository<T> where T : class
    {
        Task<ElasticResponse<T>> SearchAggregateByCountryId(GraphCriteria criteria);
        Task<ElasticResponse<T>> Search(LogSearchCriteria criteria);
        Task<ElasticResponse<T>> SearchAggregate(SearchAgreggateCriteria criteria);
        Task<ElasticResponse<T>> Find(LogSearchCriteria criteria);
        Task Create(T product);
        Task Delete(LogCriteria criteria);
        Task Bulk(IEnumerable<T> records);


        
    }
}
