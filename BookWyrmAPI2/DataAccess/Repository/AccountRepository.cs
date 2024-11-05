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
            var user = await _userManager.FindByNameAsync(username); // Use FindByNameAsync with username
            if (user == null)
            {
                return null; // Handle the user-not-found case
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

        // Update user profile using UserProfileDto
        public async Task<IdentityResult> UpdateUserProfileAsync(string username, UserProfileDto userProfileDto)
        {
            var user = await FindUserByUsernameAsync(username);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Update properties only if they are provided in the DTO
            if (userProfileDto.FirstName != null) user.FirstName = userProfileDto.FirstName;
            if (userProfileDto.LastName != null) user.LastName = userProfileDto.LastName;
            if (userProfileDto.TelNumber != null) user.TelNumber = userProfileDto.TelNumber;
            if (userProfileDto.Address != null) user.Address = userProfileDto.Address;
            if (userProfileDto.City != null) user.City = userProfileDto.City;
            if (userProfileDto.State != null) user.State = userProfileDto.State;
            if (userProfileDto.ZipCode != null) user.ZipCode = userProfileDto.ZipCode;

            // Save changes
            return await _userManager.UpdateAsync(user);
        }

        public async Task<List<int>> GetBooksInCartAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new List<int>(); // Return an empty list if the user does not exist
            }

            return user.BookIds ?? new List<int>(); // Return the BookIds or an empty list if null
        }

        public async Task<IdentityResult> UpdateBooksInCartAsync(string username, List<int> bookIds)
        {
            var user = await FindUserByUsernameAsync(username);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            user.BookIds = bookIds; // Update the BookIds
            return await _userManager.UpdateAsync(user); // Save changes
        }
    }
}
