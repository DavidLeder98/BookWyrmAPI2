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
            var books = await _bookRepository.GetBookListAsync(searchTerm);
            return Ok(books);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookCardDto>>> GetBooks(string? searchTerm, SortBy sortBy = SortBy.Id)
        {
            var books = await _bookRepository.GetBookCardsAsync(searchTerm, sortBy);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromForm] BookCreateDto bookCreateDto)
        {
            var book = await _bookRepository.CreateBookAsync(bookCreateDto);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        [HttpPut]
        public async Task<ActionResult<Book>> UpdateBook([FromForm] BookUpdateDto bookUpdateDto)
        {
            var updatedBook = await _bookRepository.UpdateBookAsync(bookUpdateDto);
            if (updatedBook == null) return NotFound();

            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var deletedBook = await _bookRepository.DeleteBookAsync(id);
            if (deletedBook == null) return NotFound();

            return Ok(deletedBook);
        }
    }
}
