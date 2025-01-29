using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FinalWebApi.Filters;
using FinalWebApi.Models;
using MySql.Data.MySqlClient;

namespace FinalWebApi.Controllers
{
    //[JWTAuthorizationFilter("Admin")]
    public class UserController : ApiController
    {
        private readonly string _connectionString;

        public UserController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        [HttpPost]
        [Route("api/AddUser")]
        public IHttpActionResult Post(UserModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO user (id,username,password,role)  VALUES (@id,@username,@password,@role);";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", user.Id);
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@role", user.Role);

                    command.ExecuteNonQuery();
                }
            }
            return Ok("User created");
        }

        [HttpGet]
        [Route("api/GetUsers")]
        public IHttpActionResult Get()
        {
            List<UserModel> users = new List<UserModel>();
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM user";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserModel
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Username = reader["username"].ToString(),
                                Password = reader["password"].ToString(),
                                Role = reader["role"].ToString()
                            });
                        }
                    }
                }
            }
            return Ok(users);
        }



    }
}
