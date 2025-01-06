using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using JWTInWebAPI.Models;

/// <summary>
/// Controller for handling authentication-related actions like login and JWT token generation.
/// </summary>
[RoutePrefix("api/auth")]
public class AuthController : ApiController
{
    private string secretKey = "RuyekAvdaras417107701741SaradvaKeyur"; 

    /// <summary>
    /// Logs in the user by validating their credentials and generates a JWT token for authorized users.
    /// If credentials are valid, a token is returned; otherwise, the user is unauthorized.
    /// </summary>
    /// <param name="login">The login model containing the username and password.</param>
    /// <returns>An IHttpActionResult containing the generated JWT token or an Unauthorized response.</returns>
    [HttpPost]
    [Route("login")]
    public IHttpActionResult Login([FromBody] LoginModel login)
    {
        // Validate user credentials (this is just a simple example)
        if (login.Username == "admin" && login.Password == "password")
        {
            var token = GenerateJwtToken(login.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }

    /// <summary>
    /// Generates a JWT token for the specified username, with a role claim and expiration.
    /// </summary>
    /// <param name="username">The username for which the token is being generated.</param>
    /// <returns>A string representing the generated JWT token.</returns>
    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin") // Example of adding a role claim
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
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
