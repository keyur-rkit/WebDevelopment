using AuthorizationInWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AuthorizationInWebAPI.Auth
{
    /// <summary>
    /// Custom authorization filter for basic authentication.
    /// This attribute validates the user's credentials from the Authorization header in the request.
    /// </summary>
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// OnAuthorization method to handle authentication by checking the Authorization header for valid credentials.
        /// If the credentials are valid, it sets up the principal with claims. 
        /// Otherwise, it returns an unauthorized response.
        /// </summary>
        /// <param name="actionContext">The context of the HTTP action being executed.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateErrorResponse(HttpStatusCode.Unauthorized, "Login failed");
            }
            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                // username:password base64 encoded
                // NormalUser:12345 Tm9ybWFsVXNlcjoxMjM0NQ
                // AdminUser:12345 QWRtaW5Vc2VyOjEyMzQ1
                // SuperAdminUser:12345 U3VwZXJBZG1pblVzZXI6MTIzNDU=

                string decodedAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string[] usernamepassword = decodedAuthToken.Split(':');

                string username = usernamepassword[0];
                string password = usernamepassword[1];

                if (Login(username, password))
                {
                    var userDetails = GetUserDetails(username, password);
                    var identity = new GenericIdentity(username);
                    identity.AddClaim(new Claim(ClaimTypes.Name, userDetails.UserName));
                    identity.AddClaim(new Claim("Id", Convert.ToString(userDetails.Id)));

                    IPrincipal principal = new GenericPrincipal(identity, userDetails.Roles.Split(','));

                    Thread.CurrentPrincipal = principal;
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request
                            .CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization Denied");
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request
                        .CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Credentials");
                }
            }
        }

        /// <summary>
        /// Validates the username and password by checking them against the list of users.
        /// </summary>
        /// <param name="username">The username provided for authentication.</param>
        /// <param name="password">The password provided for authentication.</param>
        /// <returns>Returns true if the credentials match an existing user, otherwise false.</returns>
        public static bool Login(string username, string password)
        {
            return User.GetUsers().Any(user => user.UserName == username && user.Password == password);
        }

        /// <summary>
        /// Retrieves the user details based on the username and password provided.
        /// </summary>
        /// <param name="username">The username to fetch the user details.</param>
        /// <param name="password">The password to verify the user.</param>
        /// <returns>The user details if the credentials are valid, otherwise null.</returns>
        public static User GetUserDetails(string username, string password)
        {
            return User.GetUsers().FirstOrDefault(user => user.UserName ==username && user.Password == password);
        }
    }
}
