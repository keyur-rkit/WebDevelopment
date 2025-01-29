using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalWebApi.Models
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
        /// Bookname
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// Book Category
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Book Author
        /// </summary>
        public string Author { get; set; }
    }
}