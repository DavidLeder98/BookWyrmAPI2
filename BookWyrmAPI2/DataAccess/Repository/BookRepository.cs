using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
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

        public async Task<IEnumerable<BookListDto>> GetBookListAlphabeticallyAsync()
        {
            var books = await _context.Books
                .OrderBy(book => book.Title)
                .Select(book => new BookListDto
                {
                    Id = book.Id,
                    Title = book.Title
                })
                .ToListAsync();

            return books;
        }

        public async Task<IEnumerable<BookCardDto>> GetBookCardsAsync()
        {
            var books = await _context.Books
                .Select(book => new BookCardDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Rating = book.Rating,
                    BestSeller = book.BestSeller,
                    ImageUrl = book.ImageUrl,
                    ListPrice = book.ListPrice,
                    Price = book.Price
                })
                .ToListAsync();

            return books;
        }
    }
}
