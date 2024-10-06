using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.DataAccess.Repository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.AuthorDTOs;
using Microsoft.AspNetCore.Mvc;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

namespace BookWyrmAPI2.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorController> _logger;
        public AuthorController(IAuthorRepository authorRepository, ILogger<AuthorController> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        // GET: api/author
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorListDto>>> GetAuthorsList([FromQuery] string? searchTerm)
        {
            try
            {
                var authors = await _authorRepository.GetAuthorsListAsync(searchTerm);

                if (authors == null || !authors.Any())
                {
                    return NotFound("No authors found matching the search term.");
                }

                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving authors.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving authors.");
            }
        }

        // GET: api/author/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorWithBooksDto>> GetAuthorById(int id, [FromQuery] BookRepository.SortBy sortBy = BookRepository.SortBy.Id)
        {
            try
            {
                var authorWithBooks = await _authorRepository.GetAuthorByIdAsync(id, sortBy);

                if (authorWithBooks == null)
                {
                    return NotFound($"Author with ID {id} not found.");
                }

                return Ok(authorWithBooks);
            }
            catch (Exception ex)
            {
                _logger.LogError("Author with ID {Id} not found.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the author.");
            }
        }

        // POST: api/author
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Author>> CreateAuthor(AuthorCreateDto authorCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 if the model is invalid
                }

                var createdAuthor = await _authorRepository.CreateAuthorAsync(authorCreateDto);

                // Return 201 Created with the created author data
                return CreatedAtAction(nameof(GetAuthorById), new { id = createdAuthor.Id }, createdAuthor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the author.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the author.");
            }
        }

        // PUT: api/author/{id}
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Author>> UpdateAuthor(int id, AuthorUpdateDto authorUpdateDto)
        {
            try
            {
                if (id != authorUpdateDto.Id || !ModelState.IsValid)
                {
                    return BadRequest(); // Return 400 if the ID mismatch or model is invalid
                }

                var updatedAuthor = await _authorRepository.UpdateAuthorAsync(authorUpdateDto);

                if (updatedAuthor == null)
                {
                    return NotFound(); // Return 404 if the author was not found
                }

                return Ok(new { message = "Author updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the author with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the author.");
            }
        }

        // DELETE: api/author/{id}
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            try
            {
                var deletedAuthor = await _authorRepository.DeleteAuthorAsync(id);

                if (deletedAuthor == null)
                {
                    return NotFound(); // Return 404 if the author was not found
                }

                return Ok(new { message = "Author deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the author with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the author.");
            }
        }
    }
}
