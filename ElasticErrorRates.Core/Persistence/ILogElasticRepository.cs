using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticRepository<T> where T : class
    {
        Task<ElasticResponse<T>> Search(int page, int pageSize, string httpUrl);
        Task<ElasticResponse<T>> SearchAggregate();
        Task<ElasticResponse<T>> Find(string columnField, string httpUrl, string term);
        Task Create(T product);
        Task Delete(T product);
        Task Bulk(IEnumerable<Log> records);

        
    }
}
