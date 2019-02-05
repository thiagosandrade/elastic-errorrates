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
        Task<ElasticResponse<T>> SearchLogsAggregateByCountryId(GraphCriteria criteria);
        Task<ElasticResponse<T>> SearchLogsAggregate(SearchAgreggateCriteria criteria);
        Task<ElasticResponse<T>> SearchLogsDetailed(LogSearchCriteria criteria);
        Task<ElasticResponse<T>> Find(LogSearchCriteria criteria);
        Task Create(T product);
        Task Delete(LogCriteria criteria);
        Task Bulk(IEnumerable<T> records);
    }
}
