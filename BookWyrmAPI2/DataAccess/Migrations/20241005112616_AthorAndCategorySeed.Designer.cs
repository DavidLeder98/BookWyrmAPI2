﻿// <auto-generated />
using System;
using BookWyrmAPI2.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookWyrmAPI2.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241005112616_AthorAndCategorySeed")]
    partial class AthorAndCategorySeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Alchemist, wizard, frog. He does it all!",
                            Name = "Gandalor Toadbottom"
                        },
                        new
                        {
                            Id = 2,
                            Description = "One of the most respected experts in the field of dragon taming and dragon hunting.",
                            Name = "Argus The Armless"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Where did she come from? What is she hiding?",
                            Name = "Mariana Trench"
                        },
                        new
                        {
                            Id = 4,
                            Description = "The most well known author and professor from the Abjuration school of magic!",
                            Name = "Saif T. Ferst"
                        });
                });

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<bool>("BestSeller")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ListPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            BestSeller = true,
                            Description = "The best textbook on the market for aspiring alchemists!",
                            ImageUrl = "Brew.png",
                            ListPrice = 29.99m,
                            Price = 24.99m,
                            Rating = 4.0999999999999996,
                            Title = "Brewing for Beginners"
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 2,
                            BestSeller = true,
                            Description = "Master Arguses magnum opus based on 30 years of dragon handling experience!",
                            ImageUrl = "Dragon.png",
                            ListPrice = 39.99m,
                            Price = 35.99m,
                            Rating = 4.7999999999999998,
                            Title = "Dragon Taming Safety Guide"
                        },
                        new
                        {
                            Id = 3,
                            AuthorId = 3,
                            BestSeller = false,
                            Description = "Ever wondered what lurks in the darkest, deepest corners of unexplored waters?",
                            ImageUrl = "Deep.png",
                            ListPrice = 29.99m,
                            Price = 27.99m,
                            Rating = 4.4000000000000004,
                            Title = "Secrets of the Deep"
                        },
                        new
                        {
                            Id = 4,
                            AuthorId = 4,
                            BestSeller = false,
                            Description = "So you've chosen the most boring school of magic and you're worried you won't pass your exams because the dry study material? Worry now longer, with this book!",
                            ImageUrl = "Abj.png",
                            ListPrice = 26.99m,
                            Price = 25.99m,
                            Rating = 4.4000000000000004,
                            Title = "Abjuration Spells and Such"
                        },
                        new
                        {
                            Id = 5,
                            AuthorId = 1,
                            BestSeller = false,
                            Description = "You're broke and down on your luck? Why not study alchemy? Maybe it's a better career path than gambling!",
                            ImageUrl = "Gold.png",
                            ListPrice = 72.99m,
                            Price = 69.99m,
                            Rating = 2.3999999999999999,
                            Title = "Alchemy of Gold"
                        });
                });

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.BookCategory", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("BookId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BookCategories");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            CategoryId = 4
                        },
                        new
                        {
                            BookId = 2,
                            CategoryId = 3
                        },
                        new
                        {
                            BookId = 2,
                            CategoryId = 5
                        },
                        new
                        {
                            BookId = 3,
                            CategoryId = 1
                        },
                        new
                        {
                            BookId = 3,
                            CategoryId = 2
                        },
                        new
                        {
                            BookId = 3,
                            CategoryId = 5
                        },
                        new
                        {
                            BookId = 4,
                            CategoryId = 1
                        },
                        new
                        {
                            BookId = 4,
                            CategoryId = 3
                        },
                        new
                        {
                            BookId = 5,
                            CategoryId = 1
                        },
                        new
                        {
                            BookId = 5,
                            CategoryId = 4
                        });
                });

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Spellcasting"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Traveling"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Combat"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Alchemy"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Bestiary"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.Book", b =>
                {
                    b.HasOne("BookWyrmAPI2.Models.BaseModels.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.BookCategory", b =>
                {
                    b.HasOne("BookWyrmAPI2.Models.BaseModels.Book", "Book")
                        .WithMany("BookCategories")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookWyrmAPI2.Models.BaseModels.Category", "Category")
                        .WithMany("BookCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BookWyrmAPI2.Models.BaseModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BookWyrmAPI2.Models.BaseModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookWyrmAPI2.Models.BaseModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BookWyrmAPI2.Models.BaseModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.Book", b =>
                {
                    b.Navigation("BookCategories");
                });

            modelBuilder.Entity("BookWyrmAPI2.Models.BaseModels.Category", b =>
                {
                    b.Navigation("BookCategories");
                });
#pragma warning restore 612, 618
        }
    }
}