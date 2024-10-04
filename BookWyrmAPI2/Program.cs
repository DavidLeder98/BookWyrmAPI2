
using BookWyrmAPI2.DataAccess;
using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.DataAccess.Repository;
using BookWyrmAPI2.Models.BaseModels;
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
            builder.Services.AddScoped<IBookRepository, BookRepository>();

            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // ------------------------------------------------ IDENTITY ------------------------------------------------

            // Add Identity with default cookie-based authentication
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders(); // Handles cookie authentication

            // Ensure you have RoleManager<IdentityRole> registered
            builder.Services.AddScoped<RoleManager<IdentityRole>>();

            // Configure cookie settings (optional adjustments)
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Adjust timeout
                options.LoginPath = "/api/account/login";          // Path for login
                options.AccessDeniedPath = "/api/account/accessdenied";  // Path for access denied
                options.SlidingExpiration = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(options => { options.LoginPath = "/account/login"; });

            builder.Services.AddAuthorization();

            // ------------------------------------------------ IDENTITY ------------------------------------------------

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()  // Allow any origin
                               .AllowAnyMethod()  // Allow any method (GET, POST, etc.)
                               .AllowAnyHeader(); // Allow any header
                    });
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //CORS
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
