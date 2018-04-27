using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Persistence.Repository;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticRepository<T> where T : class
    {
        Task<ElasticResponse<Log>> Search(int page, int pageSize, string httpUrl);
        Task<ElasticResponse<LogSummary>> SearchAggregate();
        Task<ElasticResponse<Log>> Find(string term, bool sort, bool match);
        Task Create(T product);
        Task Delete(T product);
        Task Bulk(IEnumerable<Log> records);

        
    }
}
