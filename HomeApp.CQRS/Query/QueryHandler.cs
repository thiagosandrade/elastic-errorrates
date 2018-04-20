using System;
using System.Threading.Tasks;
using ElasticSearch.Core.CQRS.Query;

namespace ElasticSearch.CQRS.Query
{
    public class QueryHandler : IQueryHandler
    {
        public async Task<TResult> RetrieveAsync<TResult>(Func<Task<TResult>> query)
        {
            try
            {
                return await Task.Run(async () => await query());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TResult> RetrieveAsync<TRequest, TResult>(Func<TRequest, Task<TResult>> query, TRequest request)
        {
            try
            {
                return await Task.Run(async () => await query(request));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
