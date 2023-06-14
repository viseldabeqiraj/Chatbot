using Chatbot.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Chatbot.Caching
{
    public class CacheService : ICacheService
    {
        private IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void AddItem(string key, object value)
        {
            _memoryCache.Set(key, value);
        }

        public bool TryGetItem<TItem>(string key, out TItem item)
        {
            item = _memoryCache.Get<TItem>(key);
            return item != null;
        }

        public void ClearCache()
        {
            _memoryCache.Dispose();
        }
    }
}
