using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using BookWyrmAPI2.Models.DTOs.CategoryDTOs;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BookWyrmAPI2.DataAccess.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public enum SortBy
        {
            PriceAscending = 0,
            PriceDescending = 1,
            Title = 2,
            Rating = 3,
            Id = 4 // Default
        }

        public async Task<IEnumerable<BookListDto>> GetBookListAsync(string? searchTerm)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm) || b.Author.Name.Contains(searchTerm));
            }

            return await query.Select(b => new BookListDto
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author.Name
            }).ToListAsync();
        }

        public async Task<IEnumerable<BookCardDto>> GetBookCardsAsync(string? searchTerm, SortBy sortBy)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm) || b.Author.Name.Contains(searchTerm));
            }

            var books = await query.Select(b => new BookCardDto
            {
                Id = b.Id,
                Title = b.Title,
                Rating = b.Rating,
                BestSeller = b.BestSeller,
                ListPrice = b.ListPrice,
                Price = b.Price,
                ImageUrl = b.ImageUrl,
                AuthorName = b.Author.Name
            }).ToListAsync();

            return sortBy switch
            {
                SortBy.PriceAscending => books.OrderBy(b => b.Price),
                SortBy.PriceDescending => books.OrderByDescending(b => b.Price),
                SortBy.Title => books.OrderBy(b => b.Title),
                SortBy.Rating => books.OrderByDescending(b => b.Rating),
                _ => books.OrderBy(b => b.Id) // default sort by Id
            };
        }

        public async Task<BookDetailsDto> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .Where(b => b.Id == id)
                .Select(b => new BookDetailsDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Rating = b.Rating,
                    BestSeller = b.BestSeller,
                    ListPrice = b.ListPrice,
                    Price = b.Price,
                    ImageUrl = b.ImageUrl,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown",
                    Categories = b.BookCategories.Select(bc => new CategoryListDto
                    {
                        Id = bc.Category.Id,
                        Name = bc.Category.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            return book ?? null;
        }

        public async Task<Book> CreateBookAsync(BookCreateDto bookCreateDto)
        {
            var book = new Book
            {
                Title = bookCreateDto.Title,
                Description = bookCreateDto.Description,
                Rating = bookCreateDto.Rating,
                BestSeller = bookCreateDto.BestSeller,
                ListPrice = bookCreateDto.ListPrice,
                Price = bookCreateDto.Price,
                AuthorId = bookCreateDto.AuthorId
            };

            // Handle image upload
            if (bookCreateDto.ImageFile != null)
            {
                book.ImageUrl = await SaveImageAsync(bookCreateDto.ImageFile);
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Link categories
            if (bookCreateDto.CategoryIds.Any())
            {
                var bookCategories = bookCreateDto.CategoryIds.Select(catId => new BookCategory
                {
                    BookId = book.Id,
                    CategoryId = catId
                });
                await _context.BookCategories.AddRangeAsync(bookCategories);
                await _context.SaveChangesAsync();
            }

            return book;
        }

        public async Task<Book> UpdateBookAsync(BookUpdateDto bookUpdateDto)
        {
            var book = await _context.Books.FindAsync(bookUpdateDto.Id);
            if (book == null) return null;

            book.Title = bookUpdateDto.Title;
            book.Description = bookUpdateDto.Description;
            book.Rating = bookUpdateDto.Rating;
            book.BestSeller = bookUpdateDto.BestSeller;
            book.ListPrice = bookUpdateDto.ListPrice;
            book.Price = bookUpdateDto.Price;
            book.AuthorId = bookUpdateDto.AuthorId;

            // Handle image upload
            if (bookUpdateDto.ImageFile != null)
            {
                book.ImageUrl = await SaveImageAsync(bookUpdateDto.ImageFile);
            }

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            // Update categories
            var existingCategories = await _context.BookCategories
                .Where(bc => bc.BookId == book.Id)
                .ToListAsync();

            _context.BookCategories.RemoveRange(existingCategories);
            await _context.SaveChangesAsync();

            if (bookUpdateDto.CategoryIds.Any())
            {
                var bookCategories = bookUpdateDto.CategoryIds.Select(catId => new BookCategory
                {
                    BookId = book.Id,
                    CategoryId = catId
                });
                await _context.BookCategories.AddRangeAsync(bookCategories);
                await _context.SaveChangesAsync();
            }

            return book;
        }

        public async Task<Book> DeleteBookAsync(int id)
        {
            var book = await _context.Books.Include(b => b.BookCategories).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return null;

            // Remove any associated BookCategories
            var bookCategories = _context.BookCategories.Where(bc => bc.BookId == id);
            _context.BookCategories.RemoveRange(bookCategories);

            // Remove the book
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {

            if (imageFile == null || imageFile.Length == 0) return null;

            // Define the directory to save images
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/books");
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName); // Generate a unique file name
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Ensure the uploads folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Save the image file
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // Optionally return null or throw an exception
                throw new Exception("Could not save image.", ex);
            }

            // Return the URL of the saved image
            return $"/uploads/books/{fileName}";
        }
    }
}
