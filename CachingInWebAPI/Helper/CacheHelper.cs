using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace CachingInWebAPI.Helper
{
    /// <summary>
    /// Helper class for cache management.
    /// Provides methods for getting, setting, and removing cache items.
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// Retrieves an item from the cache by its key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>The cached object, or null if not found.</returns>
        public static object Get(string key)
        {
            return HttpContext.Current.Cache[key];
        }

        /// <summary>
        /// Inserts an item into the cache with a key, value, and duration.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The value to store.</param>
        /// <param name="duration">Cache duration.</param>
        public static void Set(string key, object value, TimeSpan duration)
        {
            HttpContext.Current.Cache.Insert(key, value, null, DateTime.MaxValue, duration);
        }

        /// <summary>
        /// Removes an item from the cache by its key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public static void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
    }
}