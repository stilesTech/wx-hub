using System;
using Microsoft.Extensions.Caching.Memory;

namespace Common
{
    public class CacheHelper
    {

        private static MemoryCache memoryCache;

        static CacheHelper()
        {
            //缓存配置
            MemoryCacheOptions options = new MemoryCacheOptions()
            {
                SizeLimit = 100,
                CompactionPercentage = 0.2,
                ExpirationScanFrequency = TimeSpan.FromSeconds(3)
            };
            memoryCache = new MemoryCache(options);

            ////单个缓存项的配置
            //MemoryCacheEntryOptions cacheEntityOps = new MemoryCacheEntryOptions()
            //{
            //    //绝对过期时间
            //    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(2)),

            //    //相对过期时间
            //    //SlidingExpiration = TimeSpan.FromSeconds(3),
            //    //优先级，当缓存压缩时会优先清除优先级低的缓存项
            //    Priority = CacheItemPriority.Low,//Low,Normal,High,NeverRemove
            //                                     //缓存大小占1份
            //    Size = 1
            //};
        }


        public static void Set<T>(string key, T value, int seconds)
        {
            var options = getMemoryCacheEntryOptions(seconds);
            memoryCache.Set<T>(key, value, options);
        }

        public static T Get<T>(string key)
        {
            return memoryCache.Get<T>(key);
        }

        private static MemoryCacheEntryOptions getMemoryCacheEntryOptions(int seconds)
        {
            MemoryCacheEntryOptions cacheEntityOps = new MemoryCacheEntryOptions()
            {
                //绝对过期时间
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(seconds)),
                //相对过期时间
                //SlidingExpiration = TimeSpan.FromSeconds(seconds),
                //优先级，当缓存压缩时会优先清除优先级低的缓存项
                Priority = CacheItemPriority.Low,//Low,Normal,High,NeverRemove
                                                 //缓存大小占1份
                Size = 1
            };
            return cacheEntityOps;
        }
    }
}
