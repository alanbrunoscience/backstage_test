using Microsoft.Extensions.Caching.Memory;
namespace Jazz.Covenant.Service.CacheMemory
{
    public class CacheMemoryService : ICacheMemoryService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheMemoryService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public async Task<object> Get(object key)
        {
            return _memoryCache.Get(key);
        }
        public void Set(object key, object data,int timeExperient)
        {
            _memoryCache.Set(key, data,TimeSpan.FromMinutes(timeExperient)) ;
        }
    }
}
