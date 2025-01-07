using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;

namespace FinalWebApi.Helpers
{
    /// <summary>
    /// Helper class for generating and validating JWT tokens.
    /// </summary>
    public class JWTHelper 
    {
        private const string SecretKey = "RuyekAvdaras417107701741SaradvaKeyur"; // The secret key used for token validation.

        /// <summary>
        /// Validates the JWT token and returns the ClaimsPrincipal if valid.
        /// </summary>
        /// <param name="token">The JWT token to be validated.</param>
        /// <returns>A ClaimsPrincipal if the token is valid, otherwise null.</returns>
        public static ClaimsPrincipal ValidateJwtToken(string token)
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

        /// <summary>
        /// Generates a JWT token for the specified username, with role claim and expiration.
        /// </summary>
        /// <param name="username">The username for which the token is being generated.</param>
        /// <param name="role">The role to assign to the user in the token.</param>
        /// <returns>A string representing the generated JWT token.</returns>
        public static string GenerateJwtToken(string username, string role)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role) // Dynamically setting the role
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Issuer",
                audience: "Audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}