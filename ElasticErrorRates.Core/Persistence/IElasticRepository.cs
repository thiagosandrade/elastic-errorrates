using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;

namespace ElasticErrorRates.Core.Persistence
{
    public interface IElasticRepository<T> where T : class
    {
        Task<ElasticResponse<Shakespeare>> Search();
        Task<ElasticResponse<Shakespeare>> Find(string term, bool sort, bool match);
        Task Create(Shakespeare product);
        Task Delete(Shakespeare product);

    }
}
