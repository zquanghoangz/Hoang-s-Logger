using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Logger.Cross.Common
{
    public static class MemoryCacher
    {
        public static bool ExistKey(string key)
        {
            return GetValue(key) != null;
        }

        public static object GetValue(string key)
        {
            var memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }

        public static bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            var memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }

        public static void Update(string key, object value)
        {
            var memoryCache = MemoryCache.Default;
            memoryCache[key] = value;
        }

        public static void Delete(string key)
        {
            var memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
    }

    public class CacheObject
    {
        public string ApplicationId { get; set; }
        public List<DateTimeOffset> RequestsTime { get; set; }
        public DateTimeOffset TimeExceededLimit { get; set; }
        public bool IsExceededLimit { get; set; }
    }
}