using AuthorizationInWebAPI.Auth;
using AuthorizationInWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace BasicAuth.API.Controllers
{
    /// <summary>
    /// Controller for managing employee-related operations. 
    /// Implements basic authentication and authorization based on user roles.
    /// </summary>
    [RoutePrefix("api/employees")]
    [BasicAuthenticationAttribute]
    public class EmployeesController : ApiController
    {
        /// <summary>
        /// Retrieves a subset of employees with ID less than 3. 
        /// This action is accessible by users with the "User" role.
        /// </summary>
        /// <returns>An HttpResponseMessage containing a list of employees.</returns>
        [Route("GetFewEmployees")]
        [BasicAuthorizationAttribute(Roles = "User")]
        public HttpResponseMessage GetFewEmployees()
        {
            return Request.CreateResponse(HttpStatusCode.OK, Employee.GetEmployees().Where(e => e.Id < 3));
        }

        /// <summary>
        /// Retrieves a subset of employees with ID less than 6. 
        /// This action is accessible by users with the "Admin" role.
        /// </summary>
        /// <returns>An HttpResponseMessage containing a list of employees.</returns>
        [Route("GetMoreEmployees")]
        [BasicAuthorizationAttribute(Roles = "Admin")]
        public HttpResponseMessage GetMoreEmployees()
        {
            return Request.CreateResponse(HttpStatusCode.OK, Employee.GetEmployees().Where(e => e.Id < 6));
        }

        /// <summary>
        /// Retrieves all employees. 
        /// This action is accessible by users with the "SuperAdmin" role.
        /// </summary>
        /// <returns>An HttpResponseMessage containing a list of all employees.</returns>
        [Route("GetAllEmployees")]
        [BasicAuthorizationAttribute(Roles = "SuperAdmin")]
        public HttpResponseMessage GetAllEmployees()
        {
            return Request.CreateResponse(HttpStatusCode.OK, Employee.GetEmployees());
        }
    }
}
