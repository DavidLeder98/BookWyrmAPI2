using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookWyrmAPI2.DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var user = new AppUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginDto model)
        {
            return await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return null;
            }

            return new UserProfileDto
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TelNumber = user.TelNumber,
                Address = user.Address,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode
            };
        }

        public async Task<AppUser?> FindUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> UpdateUserProfileAsync(string username, UserProfileDto userProfileDto)
        {
            var user = await FindUserByUsernameAsync(username);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            if (userProfileDto.FirstName != null) user.FirstName = userProfileDto.FirstName;
            if (userProfileDto.LastName != null) user.LastName = userProfileDto.LastName;
            if (userProfileDto.TelNumber != null) user.TelNumber = userProfileDto.TelNumber;
            if (userProfileDto.Address != null) user.Address = userProfileDto.Address;
            if (userProfileDto.City != null) user.City = userProfileDto.City;
            if (userProfileDto.State != null) user.State = userProfileDto.State;
            if (userProfileDto.ZipCode != null) user.ZipCode = userProfileDto.ZipCode;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<List<int>> GetBooksInCartAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new List<int>();
            }

            return user.BookIds ?? new List<int>();
        }

        public async Task<IdentityResult> UpdateBooksInCartAsync(string username, List<int> bookIds)
        {
            var user = await FindUserByUsernameAsync(username);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            user.BookIds = bookIds;
            return await _userManager.UpdateAsync(user);
        }
    }
}
