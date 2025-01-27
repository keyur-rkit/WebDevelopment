using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CORSinWebAPI.Controllers
{
    public class UserController : ApiController
    {
        //[EnableCors(origins: "*", headers:"*",methods:"*")]
        [HttpGet]
        [Route("api/user")]
        public IEnumerable<string> Get()
        {
            return new string[] {"Keyur","Hit","Meet"};
        }
    }
}
