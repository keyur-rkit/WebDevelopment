using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
using WebApplication.Models;

namespace WebApplication.Controllers
{

    /// <summary>
    /// Calss for BookController
    /// </summary>
    public class BookController : ApiController
    {
        #region data
        static List<Book> books = new List<Book>
        {
            new Book{ ISBN = 1001,BookName = "One Piece",Price = 100.50 },
            new Book{ ISBN = 1002,BookName = "Ikigai",Price = 500.00 },
            new Book{ ISBN = 1003,BookName = "Atomic Habits",Price = 200.50 },
            new Book{ ISBN = 1004,BookName = "Rich Dad Poor Dad",Price = 200.00 },
            new Book{ ISBN = 1005,BookName = "Paratpar",Price = 150.00 }
        };
        #endregion

        #region GetAllBooks
        /// <summary>
        /// Get method to get all books
        /// </summary>
        /// <returns>All books</returns>
        /// <response code="200">Returns the list of books.</response>
        [HttpGet]
        [Route("api/books")]
        public IEnumerable<Book> Get()
        {
            return books;
        }
        #endregion

        #region GetBookByID
        /// <summary>
        /// Overrided get method to get book by id
        /// </summary>
        /// <param name="ISBN"></param>
        /// <returns>Book with  the specified ID, or a 404 if not found.</returns>       
        /// <response code="200">Returns the books details.</response>
        /// <response code="404">Product not found.</response>
        [HttpGet]
        [Route("api/books/{ISBN}")]
        public IHttpActionResult Get(int ISBN)
        {
            Book book = books.FirstOrDefault(b => b.ISBN == ISBN);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
        #endregion

        #region PostBook
        /// <summary>
        /// Creates a new book
        /// </summary>
        /// <param name="book"></param>
        /// <returns>The created book with a 201 status code.</returns>
        /// <response code="201">Returns the created book.</response>
        /// <response code="400">If the model is invalid.</response>
        [HttpPost]
        [Route("api/books")]
        public IHttpActionResult Post([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Need all data to post.");
            }
            books.Add(book);
            return Created($"api/book/{book.ISBN}", book);
        }
        #endregion

        #region PutBook
        /// <summary>
        /// Updates an existing book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="product">The updated book data.</param>
        /// <returns>A response indicating success or failure.</returns>
        /// <response code="200">If the book was updated successfully.</response>
        /// <response code="400">If the model is invalid or if the ISBN is changed.</response>
        /// <response code="404">If the book with the specified ID was not found.</response>
        [HttpPut]
        [Route("api/books/{ISBN}")]
        public IHttpActionResult Put(int ISBN, [FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Invalid book data.");
            }

            if (book.ISBN != ISBN)
            {
                return BadRequest("ISBN cannot be modified."); 
            }

            Book existingBook = books.FirstOrDefault(b => b.ISBN == ISBN);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.BookName = book.BookName;
            existingBook.Price = book.Price;
  

            return Ok(existingBook); 
        }
        #endregion

        #region PatchBook
        /// <summary>
        /// Updates an existing book by its ID.
        /// </summary>
        /// <param name="ISBN"></param>
        /// <param name="book"></param>
        /// <returns>A response indicating success or failure.</returns>
        /// <response code="200">If the book was updated successfully.</response>
        /// <response code="400">If the model is invalid or if the ISBN is changed.</response>
        /// <response code="404">If the book with the specified ID was not found.</response>
        [HttpPatch]
        [Route("api/books/{ISBN}")]
        public IHttpActionResult Patch(int ISBN, [FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Invalid book data");
            }

            if (book.ISBN != 0 && book.ISBN != ISBN)
            {
                return BadRequest($"ISBN can't be modified {book.ISBN.GetType()}");
            }

            Book existingBook = books.FirstOrDefault(b => b.ISBN == ISBN);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.BookName = book.BookName ?? existingBook.BookName;
            if (book.Price != 0)
            {
                existingBook.Price = book.Price;
            }
            return Ok(existingBook);
        }
        #endregion

        #region DeleteBook
        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <param name="ISBN"></param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <response code="200">If the book was deleted successfully.</response>
        /// <response code="404">If the book with the specified ID was not found.</response>
        [HttpDelete]
        [Route("api/books/{ISBN}")]
        public IHttpActionResult Delete(int ISBN)
        {
            Book existingBook = books.FirstOrDefault(b => b.ISBN == ISBN);
            if (existingBook == null)
            {
                return NotFound();
            }

            books.Remove(existingBook);
            return Ok($"Book with {ISBN} deleted sucessfully");
        }
        #endregion
    }
}
