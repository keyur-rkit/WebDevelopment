using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using FinalWebApi.Models;
using FinalWebApi.Helpers;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Input;

/// <summary>
/// Controller for handling authentication-related actions like login and JWT token generation.
/// </summary>
[RoutePrefix("api/auth")]
public class AuthController : ApiController
{
    private readonly string _connectionString;

    public AuthController()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }


    /// <summary>
    /// Login method to get JWT token if user exist
    /// </summary>
    /// <param name="login">LoginModel</param>
    /// <returns>IHttpActionResult</returns>
    [HttpPost]
    [Route("login")]
    public IHttpActionResult Login([FromBody] LoginModel login)
    {
        // Validate user credentials 
        UserModel user  = GetUser(login);

        if(user == null)
        {
            return BadRequest("Invalid Username or Password");
        }

        string token = JWTHelper.GenerateJwtToken(user.Username, user.Role);
        return Ok(token);

    }

    /// <summary>
    /// Method to get user from username and password
    /// </summary>
    /// <param name="objLoginModel">LoginModel</param>
    /// <returns>UserModel</returns>
    [HttpGet]
    [Route("GetUser")]
    public UserModel GetUser(LoginModel objLoginModel)
    {
        UserModel user = new UserModel();
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();
            string query = @"SELECT * FROM user WHERE username = @username AND password = @password";
            using (MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@username", objLoginModel.Username);
                command.Parameters.AddWithValue("@password", objLoginModel.Password);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user.Id = Convert.ToInt32(reader["id"]);
                        user.Username = reader["username"].ToString();
                        user.Password = reader["password"].ToString();
                        user.Role = reader["role"].ToString();

                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
