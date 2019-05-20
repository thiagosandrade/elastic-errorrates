using System.Threading.Tasks;

namespace ElasticErrorRates.Core.Cache
{
    public interface ICacheService
    {
        Task<TResult> GetOrAddAsync<TResult>(string cacheKey, Task<TResult> action);
    }
}