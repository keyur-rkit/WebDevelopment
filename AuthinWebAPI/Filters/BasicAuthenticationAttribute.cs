using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace AuthinWebAPI.Filters
{
    /// <summary>
    /// Custom filter implementing Basic Authentication.
    /// </summary>
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Checks Authorization header, validates credentials, and sets principal.
        /// </summary>
        /// <param name="actionContext">The action context with request and response details.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Missing Headers");
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                byte[] temp = Convert.FromBase64String(authenticationToken);
                string credentials = Encoding.UTF8.GetString(temp);
                string[] credentialsValues = credentials.Split(':');
                string username = credentialsValues[0];
                string password = credentialsValues[1];

                if (IsAuthorizedUser(username, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid credential");
                }
            }
        }

        /// <summary>
        /// Validates username and password.
        /// </summary>
        /// <param name="username">The provided username.</param>
        /// <param name="password">The provided password.</param>
        /// <returns>True if valid, else false.</returns>
        private bool IsAuthorizedUser(string username, string password)
        {
            return username == "admin" && password == "password";
        }

        // Base64 encoded 'admin:password' YWRtaW46cGFzc3dvcmQ=
    }
}
