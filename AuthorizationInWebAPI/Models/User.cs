﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorizationInWebAPI.Models
{
    /// <summary>
    /// Represents a user in the system with properties such as ID, username, password, roles, and email.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Retrieves a list of predefined users with different roles for testing purposes.
        /// This method is used to simulate a data source for user management.
        /// </summary>
        /// <returns>A list of <see cref="User"/> objects representing users.</returns>
        public static List<User> GetUsers()
        {
            List<User> users = new List<User>()
            {
            new User{Id=1001,UserName="NormalUser",Password="12345",Roles="User" },
            new User{Id=1002,UserName="AdminUser",Password="12345",Roles="User,Admin"},
            new User{Id=1003,UserName="SuperAdminUser",Password="12345",Roles="User,Admin,SuperAdmin"}
            };
            return users;
        }
    }
}