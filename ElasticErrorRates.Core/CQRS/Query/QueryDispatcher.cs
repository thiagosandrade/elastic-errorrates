using ElasticErrorRates.Core.Cache;
using ElasticErrorRates.Core.Manager;

namespace ElasticErrorRates.Core.CQRS.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public QueryDispatcher(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<TResult> DispatchAsync<TRequest, TResult>(Func<TRequest, Task<TResult>> query, TRequest request, string cacheKey)
        {
            try
            {
                return await _cacheService.GetOrAddAsync(
                    cacheKey,
                    _unitOfWork.GetInstance<IQueryHandler>().RetrieveAsync(query, request));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TResult> DispatchAsync<TResult>(Func<Task<TResult>> query, string cacheKey)
        {
            try
            {
                return await _cacheService.GetOrAddAsync(
                    cacheKey,
                    _unitOfWork.GetInstance<IQueryHandler>().RetrieveAsync(query));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
