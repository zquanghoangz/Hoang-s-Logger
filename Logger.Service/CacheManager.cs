using System;
using System.Collections.Generic;
using Logger.Cross.Common;

namespace Logger.Service
{
    public static class CacheManager
    {
        public static bool ExistToken(string token)
        {
            return MemoryCacher.ExistKey(token);
        }

        public static void SaveToken(string token, string applicationId, int expirationTime)
        {
            var cache = new CacheObject
            {
                ApplicationId = applicationId,
                RequestsTime = new List<DateTimeOffset>(),
                IsExceededLimit = false
            };

            cache.RequestsTime.Add(DateTimeOffset.Now);

            //The session lifetime must be configurable in the database
            MemoryCacher.Add(token, cache, DateTimeOffset.UtcNow.AddMinutes(expirationTime));
        }

        public static bool IsExceededLimit(string token, DateTimeOffset requestTime)
        {
            var cache = MemoryCacher.GetValue(token) as CacheObject;
            if (cache != null)
            {
                if (cache.IsExceededLimit &&
                    requestTime - cache.TimeExceededLimit <= TimeSpan.FromMinutes(5))
                {
                    return true;
                }
                if (cache.IsExceededLimit &&
                    requestTime - cache.TimeExceededLimit > TimeSpan.FromMinutes(5))
                {
                    cache.IsExceededLimit = false;
                    MemoryCacher.Update(token, cache);
                    return false;
                }
            }

            return false;
        }

        public static void AddRequestTime(string token, DateTimeOffset requestTime)
        {
            var cache = MemoryCacher.GetValue(token) as CacheObject;
            if (cache != null)
            {
                while (cache.RequestsTime.Count > 0 && requestTime - cache.RequestsTime[0] > TimeSpan.FromMinutes(1))
                {
                    cache.RequestsTime.RemoveAt(0);
                }

                cache.RequestsTime.Add(requestTime);

                if (cache.RequestsTime.Count >= 60)
                {
                    cache.IsExceededLimit = true;
                    cache.TimeExceededLimit = requestTime;
                }

                MemoryCacher.Update(token, cache);
            }
        }
    }
}