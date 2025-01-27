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
    /// Custom authorization filter that implements Basic Authentication for HTTP requests.
    /// </summary>
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// OnAuthorization method is called during the authorization process. It checks the presence of an Authorization header,
        /// validates the credentials, and sets the current principal if authorized, otherwise responds with Unauthorized status.
        /// </summary>
        /// <param name="actionContext">The action context containing the request and response details.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,"Missing Headers");
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
        /// Validates the provided username and password to check if the user is authorized.
        /// </summary>
        /// <param name="username">The username provided in the request.</param>
        /// <param name="password">The password associated with the username.</param>
        /// <returns>True if the user is authorized, false otherwise.</returns>
        private bool IsAuthorizedUser(string username, string password)
        {
            // Replace this with actual validation logic, such as checking against a database
            return username == "admin" && password == "password";
        }

        // Base64 encoded 'admin:password' YWRtaW46cGFzc3dvcmQ= 
    }
}
