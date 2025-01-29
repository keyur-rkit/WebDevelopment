using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseCRUD.Models
{
    /// <summary>
    /// Book Model for Database connetcion
    /// </summary>
    public class BookModel
    {
        /// <summary>
        /// ISBN
        /// </summary>
        public int ISBN { get; set; }

        /// <summary>
        /// BookName
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        public string Author { get; set; }
    }
}