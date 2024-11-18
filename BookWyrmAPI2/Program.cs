
using BookWyrmAPI2.DataAccess;
using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.DataAccess.Repository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

namespace BookWyrmAPI2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // data access
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IBundleRepository, BundleRepository>();

            // logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // ------------------------------------------------ IDENTITY ------------------------------------------------

            // Adds Identity with default cookie-based authentication
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;

                // Sets user name validation rules
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders(); // Handles cookie authentication

            // Ensures you have RoleManager<IdentityRole> registered
            builder.Services.AddScoped<RoleManager<IdentityRole>>();

            // Configures cookie settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/api/account/login";
                options.AccessDeniedPath = "/api/account/accessdenied";
                options.SlidingExpiration = true;
            });

            // Adds custom user validation for username length
            builder.Services.AddScoped<IUserValidator<AppUser>, CustomUserValidator>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(options => { options.LoginPath = "/account/login"; });

            builder.Services.AddAuthorization();

            // ------------------------------------------------ IDENTITY ------------------------------------------------

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    corsBuilder =>
                    {
                        corsBuilder.WithOrigins(
                                "https://bwfe-vercel.vercel.app",
                                "https://bookwyrm.store",
                                "https://www.bookwyrm.store",
                                "http://localhost:7230"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();  // Allow credentials (cookies)
                    });
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configures the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //CORS
            app.UseCors("AllowFrontend");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
