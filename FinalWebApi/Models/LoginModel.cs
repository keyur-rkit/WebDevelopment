using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalWebApi.Models
{
    /// <summary>
    /// Model representing the login credentials required for user authentication.
    /// </summary>
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
