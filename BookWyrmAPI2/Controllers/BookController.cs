using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using Microsoft.AspNetCore.Mvc;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

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

        // GET api/book/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBookById(int id)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(id);

                if (book == null)
                {
                    return NotFound(); // Return 404 if the book is not found
                }

                return Ok(book); // Return 200 with the book details
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving this book.");
            }
            
        }

        // GET: api/book/getbooks
        [HttpGet("getbooks")]
        public async Task<ActionResult<IEnumerable<BookCardDto>>> GetBookCards(string? searchTerm = null, string? sortOrder = null)
        {
            try
            {
                // Default to sorting by Id
                SortBy sortByEnum = SortBy.Id;

                // Try to parse the sortOrder string to the SortBy enum
                if (!string.IsNullOrEmpty(sortOrder) && Enum.TryParse(sortOrder, true, out SortBy parsedSortBy))
                {
                    sortByEnum = parsedSortBy;
                }

                // Fetch sorted and filtered books from the repository
                var books = await _bookRepository.GetBookCardsAsync(searchTerm, sortByEnum);

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
