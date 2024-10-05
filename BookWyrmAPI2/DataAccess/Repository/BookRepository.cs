using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using BookWyrmAPI2.Models.DTOs.CategoryDTOs;
using Microsoft.EntityFrameworkCore;

namespace BookWyrmAPI2.DataAccess.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookListDto>> GetBookListAsync()
        {
            var books = await _context.Books
                .Select(book => new BookListDto
                {
                    Id = book.Id,
                    Title = book.Title
                })
                .ToListAsync();

            return books;
        }

        public async Task<BookDetailsDto> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .Where(b => b.Id == id)
                .Select(b => new BookDetailsDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Rating = b.Rating,
                    BestSeller = b.BestSeller,
                    ImageUrl = b.ImageUrl,
                    ListPrice = b.ListPrice,
                    Price = b.Price,
                    AuthorId = b.Author.Id,
                    AuthorName = b.Author.Name,
                    CategoryIds = b.BookCategories.Select(bc => bc.CategoryId).ToList(),
                    CategoryNames = b.BookCategories.Select(bc => bc.Category.Name).ToList()
                })
                .FirstOrDefaultAsync();

            return book;
        }

        public enum SortBy
        {
            Id,
            PriceAscending,
            PriceDescending,
            Rating,
            Title
        }

        public async Task<IEnumerable<BookCardDto>> GetBookCardsAsync(string? searchTerm, SortBy sortBy)
        {
            IQueryable<BookCardDto> booksQuery = _context.Books
                .Include(b => b.Author)
                .Select(book => new BookCardDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Rating = book.Rating,
                    BestSeller = book.BestSeller,
                    ImageUrl = book.ImageUrl,
                    ListPrice = book.ListPrice,
                    Price = book.Price,
                    AuthorName = book.Author != null ? book.Author.Name : "Unknown"
                });

            // Apply search filtering if a searchTerm is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // make search case-insensitive
                booksQuery = booksQuery.Where(book =>
                    book.Title.ToLower().Contains(searchTerm) ||
                    book.AuthorName!.ToLower().Contains(searchTerm));
            }

            // Apply sorting based on the sortBy parameter
            switch (sortBy)
            {
                case SortBy.PriceAscending:
                    booksQuery = booksQuery.OrderBy(book => book.Price);
                    break;
                case SortBy.PriceDescending:
                    booksQuery = booksQuery.OrderByDescending(book => book.Price);
                    break;
                case SortBy.Rating:
                    booksQuery = booksQuery.OrderByDescending(book => book.Rating);
                    break;
                case SortBy.Title:
                    booksQuery = booksQuery.OrderBy(book => book.Title);
                    break;
                case SortBy.Id:
                default: // Default to sorting by Id
                    booksQuery = booksQuery.OrderBy(book => book.Id);
                    break;
            }

            var books = await booksQuery.ToListAsync();

            return books;
        }
    }
}
