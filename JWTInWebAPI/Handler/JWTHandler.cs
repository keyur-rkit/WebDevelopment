using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Net;

namespace JWTInWebAPI.Handler
{
    /// <summary>
    /// Custom handler for processing JWT authentication for secured API endpoints.
    /// This handler intercepts the HTTP request, checks for a valid JWT token, and sets the user principal for authorization.
    /// </summary>
    public class JWTHandler : DelegatingHandler
    {
        private const string SecretKey = "RuyekAvdaras417107701741SaradvaKeyur"; // The secret key used for token validation.

        /// <summary>
        /// Intercepts the HTTP request to validate the JWT token for secured endpoints.
        /// If the token is valid, the current principal is set; otherwise, an Unauthorized response is returned.
        /// </summary>
        /// <param name="request">The HTTP request being processed.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>The HTTP response after processing the request.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Check if the request is for a secured endpoint by inspecting the route (optional)
            var isSecuredEndpoint = request.RequestUri.AbsolutePath.Contains("/api/secure/data");

            // If the endpoint requires authentication, proceed to validate the token
            if (isSecuredEndpoint)
            {
                if (request.Headers.Authorization == null)
                {
                    // Token is missing, return Unauthorized
                    return request.CreateResponse(HttpStatusCode.Unauthorized, "Token is missing. Please authenticate.");
                }

                var token = request.Headers.Authorization.Parameter;
                var principal = ValidateJwtToken(token);

                if (principal == null)
                {
                    // Invalid token, return Unauthorized
                    return request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid token.");
                }

                // Token is valid, set the current principal for authorization
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
            }

            // Continue processing the request
            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Validates the JWT token and returns the ClaimsPrincipal if valid.
        /// </summary>
        /// <param name="token">The JWT token to be validated.</param>
        /// <returns>A ClaimsPrincipal if the token is valid, otherwise null.</returns>
        private ClaimsPrincipal ValidateJwtToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = "Issuer",
                    ValidAudience = "Audience",
                    IssuerSigningKey = key
                };

                SecurityToken validatedToken;
                var principal = handler.ValidateToken(token, parameters, out validatedToken);

                return principal;
            }
            catch
            {
                return null; // Invalid token
            }
        }
    }
}
