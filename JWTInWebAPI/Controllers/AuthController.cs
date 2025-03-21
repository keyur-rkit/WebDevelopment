﻿using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using JWTInWebAPI.Models;
using JWTInWebAPI.Handler;

/// <summary>
/// Controller for handling authentication-related actions like login and JWT token generation.
/// </summary>
[RoutePrefix("api/auth")]
public class AuthController : ApiController
{

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
            var token = JWTHandler.GenerateJwtToken(login.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }

}
