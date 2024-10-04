using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookWyrmAPI2.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/book
        [HttpGet("listbyid")]
        public async Task<ActionResult<IEnumerable<BookListDto>>> GetBookList()
        {
            try
            {
                var books = await _bookRepository.GetBookListAsync();
                return Ok(books);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving books.");
            }
        }

        // GET: api/book
        [HttpGet("listalphabetically")]
        public async Task<ActionResult<IEnumerable<BookListDto>>> GetBookListAlphabetically()
        {
            try
            {
                var books = await _bookRepository.GetBookListAlphabeticallyAsync();
                return Ok(books);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving books.");
            }
        }

        // GET: api/book/cardsbyid
        [HttpGet("cardsbyid")]
        public async Task<ActionResult<IEnumerable<BookCardDto>>> GetBookCards()
        {
            try
            {
                var books = await _bookRepository.GetBookCardsAsync();

                // Create the base URL using the request's scheme, host, and path
                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}/uploads/books/";

                // Update the ImageUrl for each book to include the full URL
                foreach (var book in books)
                {
                    if (!string.IsNullOrEmpty(book.ImageUrl))
                    {
                        book.ImageUrl = $"{baseUrl}{book.ImageUrl}";
                    }
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving books.");
            }
        }
    }
}
