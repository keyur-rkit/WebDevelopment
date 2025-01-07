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
        public int ISBN { get; set; }
        public string BookName { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
    }
}