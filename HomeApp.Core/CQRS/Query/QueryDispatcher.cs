using System;
using System.Threading.Tasks;
using ElasticSearch.Core.Manager;

namespace ElasticSearch.Core.CQRS.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IUnitOfWork _unitOfWork;

        public QueryDispatcher(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResult> DispatchAsync<TRequest, TResult>(Func<TRequest, Task<TResult>> query, TRequest request)
        {
            try
            {
                return await _unitOfWork.GetInstance<IQueryHandler>().RetrieveAsync(query, request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TResult> DispatchAsync<TResult>(Func<Task<TResult>> query)
        {
            try
            {
                return await _unitOfWork.GetInstance<IQueryHandler>().RetrieveAsync(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
