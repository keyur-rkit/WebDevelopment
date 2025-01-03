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
        public int ISBN { get; set; }
        public string BookName { get; set; }
        public double Price { get; set; }
        #endregion
    }

    #endregion
}