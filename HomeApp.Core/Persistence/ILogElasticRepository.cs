using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElasticSearch.Core.Models;

namespace ElasticSearch.Core.Persistence
{
    public interface ILogElasticRepository<T> where T : class
    {
        Task<ElasticResponse<Log>> Search();
        Task<ElasticResponse<Log>> Find(string term, bool sort, bool match);
        Task Create(T product);
        Task Delete(T product);
        Task Bulk(IEnumerable<Log> records);

    }
}
