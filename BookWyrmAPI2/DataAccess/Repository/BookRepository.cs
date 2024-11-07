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
            var baseUrl = "https://bookwyrmapi2.azurewebsites.net";
            var query = _context.Books.Include(b => b.Author).AsQueryable();

            // Split the searchTerm into individual words
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTerms = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Ensure that all terms are present in either the book title or the author's name
                foreach (var term in searchTerms)
                {
                    var lowerTerm = term.ToLower(); // Convert the term to lowercase for case-insensitive search
                    query = query.Where(b => b.Title.ToLower().Contains(lowerTerm) ||
                                             b.Author.Name.ToLower().Contains(lowerTerm));
                }
            }

            // Select the necessary fields for BookCardDto
            var books = await query.Select(b => new BookCardDto
            {
                Id = b.Id,
                Title = b.Title,
                Rating = b.Rating,
                BestSeller = b.BestSeller,
                ListPrice = b.ListPrice,
                Price = b.Price,
                ImageUrl = $"{baseUrl}{b.ImageUrl}",
                AuthorName = b.Author.Name
            }).ToListAsync();

            // Apply sorting based on the SortBy parameter
            return sortBy switch
            {
                SortBy.PriceAscending => books.OrderBy(b => b.Price),
                SortBy.PriceDescending => books.OrderByDescending(b => b.Price),
                SortBy.Title => books.OrderBy(b => b.Title),
                SortBy.Rating => books.OrderByDescending(b => b.Rating),
                _ => books.OrderBy(b => b.Id) // default sort by Id
            };
        }

        public async Task<BookCardDto> GetBookCardByIdAsync(int id)
        {
            var baseUrl = "https://bookwyrmapi2.azurewebsites.net"; // Ensure this is consistent with your other methods
            var book = await _context.Books
                .Include(b => b.Author) // Include author details
                .Where(b => b.Id == id)
                .Select(b => new BookCardDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Rating = b.Rating,
                    BestSeller = b.BestSeller,
                    ListPrice = b.ListPrice,
                    Price = b.Price,
                    ImageUrl = $"{baseUrl}{b.ImageUrl}",
                    AuthorName = b.Author.Name
                })
                .FirstOrDefaultAsync(); // Use FirstOrDefaultAsync to get a single item

            return book; // This will be null if no book is found with the given ID
        }

        public async Task<BookDetailsDto> GetBookByIdAsync(int id)
        {
            var largeBaseUrl = "https://bookwyrmapi2.azurewebsites.net";

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
                    LargeImageUrl = $"{largeBaseUrl}{b.LargeImageUrl}",
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

        public async Task<string> CreateBookAsync(BookCreateDto bookCreateDto)
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

            // Handle image uploads
            if (bookCreateDto.ImageFile != null)
            {
                // Save small image to wwwroot/uploads/books
                book.ImageUrl = await SaveImageAsync(bookCreateDto.ImageFile);
            }

            if (bookCreateDto.LargeImageFile != null)
            {
                // Save large image to wwwroot/uploads/largebooks
                book.LargeImageUrl = await SaveLargeImageAsync(bookCreateDto.LargeImageFile);
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

            return "Book created successfully.";
        }

        public async Task<string> UpdateBookAsync(BookUpdateDto bookUpdateDto)
        {
            var book = await _context.Books.FindAsync(bookUpdateDto.Id);
            if (book == null) return null;

            // Update book properties
            book.Title = bookUpdateDto.Title;
            book.Description = bookUpdateDto.Description;
            book.Rating = bookUpdateDto.Rating;
            book.BestSeller = bookUpdateDto.BestSeller;
            book.ListPrice = bookUpdateDto.ListPrice;
            book.Price = bookUpdateDto.Price;
            book.AuthorId = bookUpdateDto.AuthorId;

            // Handle image upload if files are provided
            if (bookUpdateDto.ImageFile != null)
            {
                book.ImageUrl = await SaveImageAsync(bookUpdateDto.ImageFile);
            }

            if (bookUpdateDto.LargeImageFile != null)
            {
                book.LargeImageUrl = await SaveLargeImageAsync(bookUpdateDto.LargeImageFile);
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

            return "Book updated successfully.";
        }

        public async Task<string> DeleteBookAsync(int id)
        {
            var book = await _context.Books.Include(b => b.BookCategories).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return null;

            // Remove any associated BookCategories
            var bookCategories = _context.BookCategories.Where(bc => bc.BookId == id);
            _context.BookCategories.RemoveRange(bookCategories);

            // Remove the book
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return "Book deleted successfully.";
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/books");
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

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
                // You can use a logging framework here
                Console.WriteLine($"Error saving image: {ex.Message}");
                throw; // Re-throwing can be useful for higher-level error handling
            }

            return $"/uploads/books/{fileName}";
        }

        private async Task<string> SaveLargeImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            // Define the directory to save large images
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/largebooks");
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
                throw new Exception("Could not save large image.", ex);
            }

            // Return the URL of the saved large image
            return $"/uploads/largebooks/{fileName}";
        }
    }
}
