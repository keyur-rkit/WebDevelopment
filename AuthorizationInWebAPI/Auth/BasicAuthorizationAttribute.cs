using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http;

namespace AuthorizationInWebAPI.Auth
{
    /// <summary>
    /// Custom authorization attribute that extends the base AuthorizeAttribute.
    /// This attribute handles unauthorized requests by checking if the user is authenticated.
    /// </summary>
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Handles unauthorized requests by checking whether the user is authenticated.
        /// If the user is authenticated, the base class handles the request, otherwise, it returns a Forbidden status.
        /// </summary>
        /// <param name="actionContext">The context of the current HTTP action.</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}
