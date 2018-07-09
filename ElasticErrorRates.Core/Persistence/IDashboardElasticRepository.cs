using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Criteria.Dashboard;
using ElasticErrorRates.Core.Models;

namespace ElasticErrorRates.Core.Persistence
{
    public interface IDashboardElasticRepository<T> where T : class
    {
        Task<ElasticResponse<T>> Search(SearchCriteria criteria);
        Task Bulk(IEnumerable<DailyRate> records);

        
    }
}
