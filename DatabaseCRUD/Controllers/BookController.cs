using DatabaseCRUD.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System;

namespace DatabaseCRUD.Controllers
{
    /// <summary>
    /// Handles API requests related to Book data, such as retrieving, adding, updating, and deleting books.
    /// </summary>
    public class BookController : ApiController
    {
        private readonly string _connectionString;

        public BookController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        /// <summary>
        /// Retrieves a list of all books from the database.
        /// </summary>
        /// <returns>A collection of books.</returns>
        [HttpGet]
        [Route("api/books")]
        public IEnumerable<BookModel> Get()
        {
            var books = new List<BookModel>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Books";
                using (var command = new MySqlCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
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
            return books;
        }

        /// <summary>
        /// Retrieves a specific book by its ISBN from the database.
        /// </summary>
        /// <param name="ISBN">The ISBN of the book to retrieve.</param>
        /// <returns>The details of the requested book, or a 404 Not Found if the book does not exist.</returns>
        [HttpGet]
        [Route("api/books/{ISBN}")]
        public IHttpActionResult Get(int ISBN)
        {
            var book = new BookModel();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var query = $"SELECT * FROM Books WHERE ISBN = {ISBN}";
                using (var command = new MySqlCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
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
        /// <param name="book">The book object containing the details of the new book to be added.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        [HttpPost]
        [Route("api/books")]
        public IHttpActionResult Post(BookModel book)
        {
            if (book == null)
            {
                return BadRequest("Invalid book data.");
            }
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"INSERT INTO Books (ISBN, BookName, Category, Author) VALUES (@ISBN,@BookName,@Category,@Author);";
                using (var command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ISBN", book.ISBN);
                    command.Parameters.AddWithValue("@BookName", book.BookName);
                    command.Parameters.AddWithValue("@Category", book.Category);
                    command.Parameters.AddWithValue("@Author", book.Author);

                    command.ExecuteNonQuery();
                }
            }
            return Created($"api/books/{book.ISBN}", book);
        }

        /// <summary>
        /// Updates the details of an existing book in the database.
        /// </summary>
        /// <param name="ISBN">The ISBN of the book to update.</param>
        /// <param name="book">The book object containing the updated details.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        [HttpPut]
        [Route("api/books")]
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
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"UPDATE Books SET BookName = @BookName, Category = @Category , Author = @Author WHERE ISBN = @ISBN;";
                using (var command = new MySqlCommand(query, conn))
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
            return Ok(book);
        }

        /// <summary>
        /// Deletes a specific book from the database by its ISBN.
        /// </summary>
        /// <param name="ISBN">The ISBN of the book to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        [HttpDelete]
        [Route("api/books")]
        public IHttpActionResult Delete(int ISBN)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"DELETE FROM Books WHERE ISBN = @ISBN";
                using (var command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ISBN", ISBN);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return BadRequest($"There is no book with ISBN {ISBN}");
                    }
                }
            }
            return Ok($"Book with {ISBN} deleted successfully");
        }
    }
}
