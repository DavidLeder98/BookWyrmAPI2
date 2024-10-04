using BookWyrmAPI2.Models.BaseModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookWyrmAPI2.DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // seed books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Brewing for Beginners",
                    Description = "test 111",
                    Rating = 4.1,
                    BestSeller = true,
                    ImageUrl = "/uploads/books/Abj.png",
                    ListPrice = 29.99M,
                    Price = 24.99M,
                });
        }
    }
}
