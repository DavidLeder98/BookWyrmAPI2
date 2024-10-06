using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.AuthorDTOs;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using Microsoft.EntityFrameworkCore;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

namespace BookWyrmAPI2.DataAccess.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;
        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthorListDto>> GetAuthorsListAsync(string? searchTerm)
        {
            IQueryable<AuthorListDto> authorsQuery = _context.Authors
                .Select(author => new AuthorListDto
                {
                    Id = author.Id,
                    Name = author.Name
                });

            // Apply search filtering if a searchTerm is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // make search case-insensitive
                authorsQuery = authorsQuery.Where(author =>
                    author.Name.ToLower().Contains(searchTerm));
            }

            var authors = await authorsQuery.ToListAsync();

            return authors;
        }

        public async Task<AuthorWithBooksDto> GetAuthorByIdAsync(int id, SortBy sortBy)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .Where(a => a.Id == id)
                .Select(a => new AuthorWithBooksDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Books = a.Books.Select(b => new BookCardDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Rating = b.Rating,
                        BestSeller = b.BestSeller,
                        ListPrice = b.ListPrice,
                        Price = b.Price,
                        ImageUrl = b.ImageUrl,
                        AuthorName = a.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (author == null)
            {
                return null;
            }

            author.Books = sortBy switch
            {
                SortBy.PriceAscending => author.Books.OrderBy(b => b.Price).ToList(),
                SortBy.PriceDescending => author.Books.OrderByDescending(b => b.Price).ToList(),
                SortBy.Title => author.Books.OrderBy(b => b.Title).ToList(),
                SortBy.Rating => author.Books.OrderByDescending(b => b.Rating).ToList(),
                _ => author.Books.OrderBy(b => b.Id).ToList(), // default sort by Id
            };

            return author;
        }

        public async Task<Author> CreateAuthorAsync(AuthorCreateDto authorCreateDto)
        {
            var author = new Author
            {
                Name = authorCreateDto.Name,
                Description = authorCreateDto.Description
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> UpdateAuthorAsync(AuthorUpdateDto authorUpdateDto)
        {
            if (authorUpdateDto.Id <= 0) return null; // Validate ID

            var existingAuthor = await _context.Authors.FindAsync(authorUpdateDto.Id);
            if (existingAuthor == null) return null;

            existingAuthor.Name = authorUpdateDto.Name;
            existingAuthor.Description = authorUpdateDto.Description;

            _context.Authors.Update(existingAuthor);
            await _context.SaveChangesAsync();

            return existingAuthor;
        }

        public async Task<Author> DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books) // Include the related books
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return null;

            // Set the AuthorId of the associated books to null
            var books = await _context.Books
                .Where(b => b.AuthorId == id)
                .ToListAsync();

            foreach (var book in books)
            {
                book.AuthorId = null; // Set AuthorId to null
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update the books in the context
                _context.Books.UpdateRange(books);

                // Remove the author
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync(); // Commit transaction
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); // Rollback in case of error
                throw;
            }

            return author;
        }
    }
}
