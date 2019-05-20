using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ElasticErrorRates.Core.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<TResult> GetOrAddAsync<TResult>(string cacheKey, Task<TResult> action)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (cacheKey != null)
                {
                    if ( _memoryCache.TryGetValue(cacheKey, out var result))
                    {
                        return (TResult)result;
                    }

                    var absoluteExpiration = DateTime.Now;

                    result = await action;

                    _memoryCache.Set(cacheKey, result, absoluteExpiration.AddMinutes(5));
                }
                
                return await action;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    public interface ICacheService
    {
        Task<TResult> GetOrAddAsync<TResult>(string cacheKey, Task<TResult> action);
    }
}
