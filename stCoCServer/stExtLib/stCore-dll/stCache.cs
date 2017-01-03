using System;
using System.Runtime.Caching;

namespace stCore
{
    public static class stCache
    {
        public static bool GetCacheObject<T>(string key, out T obj) where T : class
        {
            if (MemoryCache.Default.Contains(key))
            {
                obj = MemoryCache.Default.Get(key) as T;
                return true;
            }
            obj = default(T);
            return false;
        }
        public static void SetCacheObject<T>(string key, T obj, DateTime exp) where T : class
        {
            if (obj != null)
            {
                CacheItemPolicy cacheIPolicy = new CacheItemPolicy();
                cacheIPolicy.AbsoluteExpiration = exp;
                MemoryCache.Default.Add(key, obj, cacheIPolicy);
            }
        }
    }
}
