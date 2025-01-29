using FinalWebApi.Models;
using FinalWebApi.Filters;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System;
using System.Data.SqlClient;
using FinalWebApi.Helpers;

namespace DatabaseCRUD.Controllers
{
    /// <summary>
    /// Controller for managing books with CRUD operations and JWT-based authorization.
    /// </summary>
    public class BookController : ApiController
    {
        private readonly string _connectionString;
        private string cacheKey = "KeyursCacheKey";

        public BookController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        /// <summary>
        /// Retrieves all books from the database. Uses caching to improve performance.
        /// </summary>
        /// <returns>A list of all books.</returns>
        [HttpGet]
        [Route("api/books")]
        [JWTAuthorizationFilter]
        public IHttpActionResult Get()
        {
            var cachedBooks = CacheHelper.Get(cacheKey);

            if (cachedBooks != null)
            {
                return Ok(cachedBooks);
            }

            List<BookModel> books = new List<BookModel>();
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Books";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new BookModel
                            {
                                ISBN = Convert.ToInt32(reader["ISBN"]),
                                BookName = reader["BookName"].ToString(),
                                Category = reader["Category"].ToString(),
                                Author = reader["Author"].ToString()
                            });
                        }
                    }
                }
            }
            CacheHelper.Set(cacheKey, books, TimeSpan.FromSeconds(30));
            return Ok(books);
        }

        /// <summary>
        /// Retrieves a specific book based on its ISBN. Uses caching to improve performance.
        /// </summary>
        /// <param name="ISBN">The ISBN of the book to retrieve.</param>
        /// <returns>The book with the specified ISBN, or a 404 if not found.</returns>
        [HttpGet]
        [Route("api/books/{ISBN}")]
        [JWTAuthorizationFilter]
        public IHttpActionResult Get(int ISBN)
        {
            BookModel book = new BookModel();
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM Books WHERE ISBN = {ISBN}";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            book.ISBN = reader.GetInt32("ISBN");
                            book.BookName = reader.GetString("BookName");
                            book.Category = reader.GetString("Category");
                            book.Author = reader.GetString("Author");
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return Ok(book);
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="book">The book details to add.</param>
        /// <returns>The created book object.</returns>
        [HttpPost]
        [Route("api/books")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult Post(BookModel book)
        {
            if (book == null)
            {
                return BadRequest("Invalid book data.");
            }
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Books (ISBN, BookName, Category, Author) VALUES (@ISBN,@BookName,@Category,@Author);";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ISBN", book.ISBN);
                    command.Parameters.AddWithValue("@BookName", book.BookName);
                    command.Parameters.AddWithValue("@Category", book.Category);
                    command.Parameters.AddWithValue("@Author", book.Author);

                    command.ExecuteNonQuery();
                }
            }
            CacheHelper.Remove(cacheKey);
            return Ok("Book added");
        }

        /// <summary>
        /// Updates the details of an existing book in the database.
        /// </summary>
        /// <param name="ISBN">The ISBN of the book to update.</param>
        /// <param name="book">The updated book details.</param>
        /// <returns>The updated book object.</returns>
        [HttpPut]
        [Route("api/books")]
        [JWTAuthorizationFilter("Admin", "Editor")]
        public IHttpActionResult Put(int ISBN, BookModel book)
        {
            if (book == null)
            {
                return BadRequest("Invalid book data.");
            }
            if (book.ISBN != ISBN)
            {
                return BadRequest("ISBN cannot be modified.");
            }
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"UPDATE Books SET BookName = @BookName, Category = @Category , Author = @Author WHERE ISBN = @ISBN;";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ISBN", book.ISBN);
                    command.Parameters.AddWithValue("@BookName", book.BookName);
                    command.Parameters.AddWithValue("@Category", book.Category);
                    command.Parameters.AddWithValue("@Author", book.Author);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return BadRequest($"There is no book with ISBN {ISBN}");
                    }
                }
            }
            CacheHelper.Remove(cacheKey);
            return Ok(book);
        }

        /// <summary>
        /// Deletes a book from the database based on its ISBN.
        /// </summary>
        /// <param name="ISBN">The ISBN of the book to delete.</param>
        /// <returns>A message indicating the success or failure of the delete operation.</returns>
        [HttpDelete]
        [Route("api/books")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult Delete(int ISBN)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"DELETE FROM Books WHERE ISBN = @ISBN";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ISBN", ISBN);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return BadRequest($"There is no book with ISBN {ISBN}");
                    }
                }
            }
            CacheHelper.Remove(cacheKey);
            return Ok($"Book with {ISBN} deleted successfully");
        }
    }
}
