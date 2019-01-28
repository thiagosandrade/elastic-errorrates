using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Criteria.Log;
using ElasticErrorRates.Core.Models;
using DashboardSearchCriteria = ElasticErrorRates.Core.Criteria.Dashboard.SearchCriteria;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticRepository<T> where T : class
    {
        Task<ElasticResponse<T>> SearchAggregateByCountryId(DashboardSearchCriteria criteria);
        Task<ElasticResponse<T>> Search(SearchCriteria criteria);
        Task<ElasticResponse<T>> SearchAggregate(SearchAgreggateCriteria criteria);
        Task<ElasticResponse<T>> Find(FindCriteria criteria);
        Task Create(T product);
        Task Delete(DashboardSearchCriteria criteria);
        Task Bulk(IEnumerable<T> records);


        
    }
}
