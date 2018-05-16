using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticRepository<T> where T : class
    {
        Task<ElasticResponse<Log>> Search(int page, int pageSize, string httpUrl);
        Task<ElasticResponse<LogSummary>> SearchAggregate();
        Task<ElasticResponse<Log>> Find(string httpUrl, string term);
        Task Create(T product);
        Task Delete(T product);
        Task Bulk(IEnumerable<Log> records);

        
    }
}
