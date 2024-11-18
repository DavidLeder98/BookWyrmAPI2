using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using BookWyrmAPI2.Models.DTOs.BundleDTOs;
using Microsoft.EntityFrameworkCore;

namespace BookWyrmAPI2.DataAccess.Repository
{
    public class BundleRepository : IBundleRepository
    {
        private readonly AppDbContext _context;
        public BundleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BundleListDto>> GetBundlesListAsync()
        {
            return await _context.Bundles
                .Select(c => new BundleListDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<BundleWithBooksDto> GetBundleByIdAsync(int id)
        {
            var baseUrl = "https://bookwyrmapi2.azurewebsites.net";

            var bundle = await _context.Bundles
                .Include(b => b.BookBundles)
                .ThenInclude(bb => bb.Book)
                .Where(b => b.Id == id)
                .Select(b => new BundleWithBooksDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Books = b.BookBundles.Select(bb => new BookCardDto
                    {
                        Id = bb.Book.Id,
                        Title = bb.Book.Title,
                        Rating = bb.Book.Rating,
                        BestSeller = bb.Book.BestSeller,
                        ListPrice = bb.Book.ListPrice,
                        Price = bb.Book.Price,
                        ImageUrl = $"{baseUrl}{bb.Book.ImageUrl}",
                        AuthorName = bb.Book.Author.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            return bundle;
        }

        public async Task<BundleWithBookListDto> GetBundleWithBookListAsync(int id)
        {
            var bundle = await _context.Bundles
                .Include(b => b.BookBundles)
                .ThenInclude(bb => bb.Book)
                .Where(b => b.Id == id)
                .Select(b => new BundleWithBookListDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Books = b.BookBundles.Select(bb => new BookListDto
                    {
                        Id = bb.Book.Id,
                        Title = bb.Book.Title,
                        AuthorName = bb.Book.Author.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            return bundle;
        }

        public async Task<Bundle> CreateBundleAsync(BundleCreateDto bundleCreateDto)
        {
            var bundle = new Bundle
            {
                Name = bundleCreateDto.Name
            };

            _context.Bundles.Add(bundle);
            await _context.SaveChangesAsync();

            // Links books to the bundle
            if (bundleCreateDto.BookIds.Any())
            {
                var bookBundles = bundleCreateDto.BookIds.Select(bookId => new BookBundle
                {
                    BundleId = bundle.Id,
                    BookId = bookId
                });
                await _context.BookBundles.AddRangeAsync(bookBundles);
                await _context.SaveChangesAsync();
            }

            return bundle;
        }

        public async Task<Bundle> UpdateBundleAsync(BundleUpdateDto bundleUpdateDto)
        {
            var bundle = await _context.Bundles.FindAsync(bundleUpdateDto.Id);
            if (bundle == null) return null;

            bundle.Name = bundleUpdateDto.Name;

            _context.Bundles.Update(bundle);
            await _context.SaveChangesAsync();

            // Updates linked books
            var existingBooks = await _context.BookBundles
                .Where(bb => bb.BundleId == bundle.Id)
                .ToListAsync();

            _context.BookBundles.RemoveRange(existingBooks);
            await _context.SaveChangesAsync();

            if (bundleUpdateDto.BookIds.Any())
            {
                var bookBundles = bundleUpdateDto.BookIds.Select(bookId => new BookBundle
                {
                    BundleId = bundle.Id,
                    BookId = bookId
                });
                await _context.BookBundles.AddRangeAsync(bookBundles);
                await _context.SaveChangesAsync();
            }

            return bundle;
        }

        public async Task<Bundle> DeleteBundleAsync(int id)
        {
            var bundle = await _context.Bundles.Include(b => b.BookBundles).FirstOrDefaultAsync(b => b.Id == id);
            if (bundle == null) return null;

            // Removes any associated BookBundles
            var bookBundles = _context.BookBundles.Where(bb => bb.BundleId == id);
            _context.BookBundles.RemoveRange(bookBundles);

            _context.Bundles.Remove(bundle);
            await _context.SaveChangesAsync();

            return bundle;
        }
    }
}
