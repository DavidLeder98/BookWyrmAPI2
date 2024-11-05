using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using BookWyrmAPI2.Models.DTOs.CategoryDTOs;
using Microsoft.EntityFrameworkCore;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

namespace BookWyrmAPI2.DataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryListDto>> GetCategoriesListAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryListDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();
        }

        public async Task<CategoryWithBooksDto> GetCategoryByIdAsync(int id, SortBy sortBy)
        {
            var baseUrl = "https://localhost:7230";

            var category = await _context.Categories
                .Include(c => c.BookCategories)
                .ThenInclude(bc => bc.Book)
                .Where(c => c.Id == id)
                .Select(c => new CategoryWithBooksDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Books = c.BookCategories.Select(bc => new BookCardDto
                    {
                        Id = bc.Book.Id,
                        Title = bc.Book.Title,
                        Rating = bc.Book.Rating,
                        BestSeller = bc.Book.BestSeller,
                        ListPrice = bc.Book.ListPrice,
                        Price = bc.Book.Price,
                        ImageUrl = $"{baseUrl}{bc.Book.ImageUrl}",
                        AuthorName = bc.Book.Author.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (category == null) return null;

            category.Books = sortBy switch
            {
                SortBy.PriceAscending => category.Books.OrderBy(b => b.Price).ToList(),
                SortBy.PriceDescending => category.Books.OrderByDescending(b => b.Price).ToList(),
                SortBy.Title => category.Books.OrderBy(b => b.Title).ToList(),
                SortBy.Rating => category.Books.OrderByDescending(b => b.Rating).ToList(),
                _ => category.Books.OrderBy(b => b.Id).ToList() // default sort by Id
            };

            return category;
        }

        public async Task<Category> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            var category = new Category
            {
                Name = categoryCreateDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var category = await _context.Categories.FindAsync(categoryUpdateDto.Id);

            if (category == null) return null;

            category.Name = categoryUpdateDto.Name;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.BookCategories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return null;

            // Remove any associated BookCategories
            var bookCategories = _context.BookCategories.Where(bc => bc.CategoryId == id);
            _context.BookCategories.RemoveRange(bookCategories);

            // Remove the category
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }
    }
}
