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
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<BookBundle> BookBundles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // author to book
            modelBuilder.Entity<Author>().HasMany(a => a.Books).WithOne(b => b.Author).HasForeignKey(a => a.AuthorId);

            // book and category
            modelBuilder.Entity<BookCategory>().HasKey(bc => new { bc.BookId, bc.CategoryId });
            modelBuilder.Entity<BookCategory>().HasOne(bc => bc.Book).WithMany(b => b.BookCategories).HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookCategory>().HasOne(bc => bc.Category).WithMany(c => c.BookCategories).HasForeignKey(bc => bc.CategoryId);

            // book and bundle
            modelBuilder.Entity<BookBundle>().HasKey(bb => new { bb.BookId, bb.BundleId });
            modelBuilder.Entity<BookBundle>().HasOne(bb => bb.Book).WithMany(book => book.BookBundles).HasForeignKey(bb => bb.BookId);
            modelBuilder.Entity<BookBundle>().HasOne(bb => bb.Bundle).WithMany(bundle => bundle.BookBundles).HasForeignKey(bb => bb.BundleId);

            // seed authors
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Gandalor Toadbottom", Description = "Alchemist, wizard, frog. He does it all!" },
                new Author { Id = 2, Name = "Argus The Armless", Description = "One of the most respected experts in the field of dragon taming and dragon hunting." },
                new Author { Id = 3, Name = "Mariana Trench", Description = "Where did she come from? What is she hiding?" },
                new Author { Id = 4, Name = "Saif T. Ferst", Description = "The most well known author and professor from the Abjuration school of magic!" });

            // seed books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Brewing for Beginners",
                    Description = "The best textbook on the market for aspiring alchemists!",
                    Rating = 4.1,
                    BestSeller = true,
                    ImageUrl = "Brew.png",
                    LargeImageUrl = "LargeBrew.png",
                    ListPrice = 29.99M,
                    Price = 24.99M,
                    AuthorId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "Dragon Taming Safety Guide",
                    Description = "Master Arguses magnum opus based on 30 years of dragon handling experience!",
                    Rating = 4.8,
                    BestSeller = true,
                    ImageUrl = "Dragon.png",
                    LargeImageUrl = "LargeDragon.png",
                    ListPrice = 39.99M,
                    Price = 35.99M,
                    AuthorId = 2
                },
                new Book
                {
                    Id = 3,
                    Title = "Secrets of the Deep",
                    Description = "Ever wondered what lurks in the darkest, deepest corners of unexplored waters?",
                    Rating = 4.4,
                    BestSeller = false,
                    ImageUrl = "Deep.png",
                    LargeImageUrl = "LargeDeep.png",
                    ListPrice = 29.99M,
                    Price = 27.99M,
                    AuthorId = 3
                },
                new Book
                {
                    Id = 4,
                    Title = "Abjuration Spells and Such",
                    Description = "So you've chosen the most boring school of magic and you're worried you won't pass your exams because the dry study material? Worry now longer, with this book!",
                    Rating = 4.4,
                    BestSeller = false,
                    ImageUrl = "Abj.png",
                    LargeImageUrl = "LargeAbj.png",
                    ListPrice = 26.99M,
                    Price = 25.99M,
                    AuthorId = 4
                },
                new Book
                {
                    Id = 5,
                    Title = "Alchemy of Gold",
                    Description = "You're broke and down on your luck? Why not study alchemy? Maybe it's a better career path than gambling!",
                    Rating = 2.4,
                    BestSeller = false,
                    ImageUrl = "Gold.png",
                    LargeImageUrl = "LargeGold.png",
                    ListPrice = 72.99M,
                    Price = 69.99M,
                    AuthorId = 1
                });

            // seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Spellcasting" },
                new Category { Id = 2, Name = "Traveling" },
                new Category { Id = 3, Name = "Combat" },
                new Category { Id = 4, Name = "Alchemy" },
                new Category { Id = 5, Name = "Bestiary" });

            // seed bookcategories
            modelBuilder.Entity<BookCategory>().HasData(
                new BookCategory { BookId = 1, CategoryId = 4 },

                new BookCategory { BookId = 2, CategoryId = 3 },
                new BookCategory { BookId = 2, CategoryId = 5 },

                new BookCategory { BookId = 3, CategoryId = 1 },
                new BookCategory { BookId = 3, CategoryId = 2 },
                new BookCategory { BookId = 3, CategoryId = 5 },

                new BookCategory { BookId = 4, CategoryId = 1 },
                new BookCategory { BookId = 4, CategoryId = 3 },

                new BookCategory { BookId = 5, CategoryId = 1 },
                new BookCategory { BookId = 5, CategoryId = 4 });

            // seed bundles
            modelBuilder.Entity<Bundle>().HasData(
                new Category { Id = 1, Name = "Popular" },
                new Category { Id = 2, Name = "Best Sellers" },
                new Category { Id = 3, Name = "10% Off" });

            // seed bookbundles
            modelBuilder.Entity<BookBundle>().HasData(
                new BookBundle { BookId = 1, BundleId = 2 },

                new BookBundle { BookId = 2, BundleId = 1 },
                new BookBundle { BookId = 2, BundleId = 3 },

                new BookBundle { BookId = 3, BundleId = 1 },
                new BookBundle { BookId = 3, BundleId = 3 },

                new BookBundle { BookId = 4, BundleId = 3 },

                new BookBundle { BookId = 5, BundleId = 2 });
        }
    }
}
