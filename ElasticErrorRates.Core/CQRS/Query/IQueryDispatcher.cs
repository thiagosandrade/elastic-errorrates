using System;
using System.Threading.Tasks;

namespace ElasticErrorRates.Core.CQRS.Query
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TRequest, TResult>(Func<TRequest, Task<TResult>> query, TRequest request);
        Task<TResult> DispatchAsync<TResult>(Func<Task<TResult>> query);
    }
}
