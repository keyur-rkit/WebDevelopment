using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace CachingInWebAPI.Helper
{
    /// <summary>
    /// Helper class for managing caching operations. 
    /// Provides methods for getting, setting, and removing items in the cache.
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// Retrieves an item from the cache by its key.
        /// Returns null if the key is not found in the cache.
        /// </summary>
        /// <param name="key">The key of the cached item to retrieve.</param>
        /// <returns>The cached object, or null if the item is not found.</returns>
        public static object Get(string key)
        {
            // Return the cached object associated with the key, or null if not found
            return HttpContext.Current.Cache[key];
        }

        /// <summary>
        /// Inserts an item into the cache with a specified key, value, and duration.
        /// The cache item will expire after the specified duration.
        /// </summary>
        /// <param name="key">The key used to store and retrieve the cached item.</param>
        /// <param name="value">The value to store in the cache.</param>
        /// <param name="duration">The duration for which the cache item will be valid.</param>
        public static void Set(string key, object value, TimeSpan duration)
        {
            // Insert the value into the cache with the specified duration and an absolute expiration time
            // Insert(string key, object value, object dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration);
            HttpContext.Current.Cache.Insert(key, value, null, DateTime.MaxValue, duration);
        }

        /// <summary>
        /// Removes an item from the cache by its key.  
        /// </summary>
        /// <param name="key">The key of the cached item to remove.</param>
        public static void Remove(string key)
        {
            // Remove the cached item associated with the key
            HttpContext.Current.Cache.Remove(key);
        }
    }
}
