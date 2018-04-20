using System;
using System.Threading.Tasks;

namespace ElasticSearch.Core.CQRS.Query
{
   
    public interface IQueryHandler
    {
        Task<TResult> RetrieveAsync<TResult>(Func<Task<TResult>> query);
        Task<TResult> RetrieveAsync<TRequest, TResult>(Func<TRequest, Task<TResult>> query, TRequest request);
    }
}
