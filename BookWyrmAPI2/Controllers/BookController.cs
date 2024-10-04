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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookListDto>>> GetBookList()
        {
            try
            {
                var books = await _bookRepository.GetAllBooksAsync(sortBy);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving books.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving books.");
            }
        }
    }
}
