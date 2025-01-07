using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JWTInWebAPI.Filters;

namespace JWTInWebAPI.Controllers
{
    /// <summary>
    /// Controller that handles secure data retrieval.
    /// </summary>
    public class DataController : ApiController
    {
        /// <summary>
        /// Retrieves secured data after successful JWT authentication.
        /// </summary>
        /// <returns>An IHttpActionResult containing secured data.</returns>
        [HttpGet]
        [Route("api/secure/data")]
        [JWTAuthorizationFilter]
        public IHttpActionResult GetData()
        {
            return Ok("This is secured data.");
        }
    }
}
