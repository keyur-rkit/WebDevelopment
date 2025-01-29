using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    #region books model
    /// <summary>
    /// Class for Books model
    /// </summary>
    public class Book
    {
        #region public property

        /// <summary>
        /// ISBN
        /// </summary>
        public int ISBN { get; set; }

        /// <summary>
        /// BookName
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public double Price { get; set; }
        #endregion
    }

    #endregion
}