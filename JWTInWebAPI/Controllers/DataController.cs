using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JWTInWebAPI.Controllers
{
    /// <summary>
    /// Controller for handling requests related to secure data. 
    /// This controller ensures that data is only accessible by authenticated users.
    /// </summary>
    public class DataController : ApiController
    {
        /// <summary>
        /// Retrieves secure data, accessible only by authenticated users.
        /// This endpoint is protected with the [Authorize] attribute, requiring a valid JWT token for access.
        /// </summary>
        /// <returns>An IHttpActionResult containing the secured data if the user is authenticated.</returns>
        [Authorize]
        [HttpGet]
        [Route("api/secure/data")]
        public IHttpActionResult GetData()
        {
            return Ok("This is secured data.");
        }
    }
}
