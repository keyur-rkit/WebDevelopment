using CachingInWebAPI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CachingInWebAPI.Controllers
{
    /// <summary>
    /// Controller responsible for handling book-related operations.
    /// Provides methods to get books, utilizing caching to improve performance.
    /// </summary>
    public class BookController : ApiController
    {
        /// <summary>
        /// Retrieves a list of books. First checks if the data is available in the cache.
        /// If the data is cached, it returns the cached data. Otherwise, it fetches the books,
        /// caches them for 10 minutes, and then returns the data.
        /// </summary>
        /// <returns>An IHttpActionResult containing a list of books.</returns>
        public IHttpActionResult GetBook()
        {
            string cacheKey = "bookCacheKey";

            var cachedBooks = CacheHelper.Get(cacheKey);

            if (cachedBooks != null)
            {
                return Ok(cachedBooks); // Return cached data
            }

            string[] books = new[] { "book1", "book2", "book3" };

            CacheHelper.Set(cacheKey, books, TimeSpan.FromMinutes(10));

            return Ok(books);
        }
    }
}
