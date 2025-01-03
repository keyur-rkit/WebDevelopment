using AuthinWebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuthinWebAPI.Controllers
{
    /// <summary>
    /// Controller that handles user-related API requests and applies Basic Authentication to secure the endpoints.
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// Retrieves a list of user-related data (dummy data in this case).
        /// The endpoint is protected with Basic Authentication, ensuring only authorized users can access the data.
        /// </summary>
        /// <returns>An IEnumerable of strings representing user data.</returns>
        [BasicAuthenticationAttribute]
        [Route("api/userdata")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Keyur", "Hit" };
        }
    }
}
