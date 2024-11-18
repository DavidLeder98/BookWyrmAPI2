using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.DataAccess.Repository;
using BookWyrmAPI2.Models.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookWyrmAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetCategoriesList()
        {
            try
            {
                var categories = await _categoryRepository.GetCategoriesListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id, [FromQuery] BookRepository.SortBy sortBy = BookRepository.SortBy.Id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id, sortBy);

                if (category == null)
                    return NotFound($"Category with id {id} not found");

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdCategory = await _categoryRepository.CreateCategoryAsync(categoryCreateDto);

                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != categoryUpdateDto.Id)
                    return BadRequest("Category ID mismatch");

                var updatedCategory = await _categoryRepository.UpdateCategoryAsync(categoryUpdateDto);

                if (updatedCategory == null)
                    return NotFound($"Category with id {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var deletedCategory = await _categoryRepository.DeleteCategoryAsync(id);

                if (deletedCategory == null)
                    return NotFound($"Category with id {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
