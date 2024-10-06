using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using BookWyrmAPI2.Models.DTOs.CategoryDTOs;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

namespace BookWyrmAPI2.DataAccess.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryListDto>> GetCategoriesListAsync();
        Task<CategoryWithBooksDto> GetCategoryByIdAsync(int id, SortBy sortBy);
        Task<Category> CreateCategoryAsync(CategoryCreateDto categoryCreateDto);
        Task<Category> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
        Task<Category> DeleteCategoryAsync(int id);
    }
}
