using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Criteria.Dashboard;
using ElasticErrorRates.Core.Models;
using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface IDashboardElasticRepository<T> where T : class
    {
        Task<ElasticResponse<T>> Search(SearchCriteria criteria);
        Task<ElasticResponse<T>> SearchAggregate(GraphCriteria criteria);
        Task Bulk(IEnumerable<DailyRate> records);

        

    }
}
