using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.DataAccess.Repository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

namespace BookWyrmAPI2.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<AuthorController> _logger;
        public BookController(IBookRepository bookRepository, ILogger<AuthorController> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookListDto>>> GetBookList(string? searchTerm)
        {
            try
            {
                var books = await _bookRepository.GetBookListAsync(searchTerm);
            return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("allbooks")]
        public async Task<ActionResult<IEnumerable<BookCardDto>>> GetBooks(string? searchTerm, SortBy sortBy = SortBy.Id)
        {
            try
            {
                var books = await _bookRepository.GetBookCardsAsync(searchTerm, sortBy);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBookById(int id)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(id);
                if (book == null) return NotFound();

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromForm] BookCreateDto bookCreateDto)
        {
            try
            {
                var book = await _bookRepository.CreateBookAsync(bookCreateDto);
                return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Book>> UpdateBook([FromForm] BookUpdateDto bookUpdateDto)
        {
            try
            {
                var updatedBook = await _bookRepository.UpdateBookAsync(bookUpdateDto);
                if (updatedBook == null) return NotFound();

                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            try
            {
                var deletedBook = await _bookRepository.DeleteBookAsync(id);
                if (deletedBook == null) return NotFound();

                return Ok(deletedBook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
