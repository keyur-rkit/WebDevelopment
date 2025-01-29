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
    /// Controller o test Basci auth
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// Retrieves a list of user-related data (dummy data in this case).
        /// </summary>
        /// <returns>An IEnumerable of strings representing user data.</returns>
        [BasicAuthentication]
        [Route("api/userdata")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Keyur", "Hit" };
        }
    }
}
