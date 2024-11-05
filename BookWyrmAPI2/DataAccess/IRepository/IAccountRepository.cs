using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;

namespace BookWyrmAPI2.DataAccess.IRepository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> RegisterAsync(RegisterDto model);
        Task<SignInResult> LoginAsync(LoginDto model);
        Task LogoutAsync();
        Task<UserProfileDto> GetUserProfileAsync(string username);
        Task<AppUser?> FindUserByUsernameAsync(string username);
        Task<IdentityResult> UpdateUserProfileAsync(string username, UserProfileDto userProfileDto);
        Task<List<int>> GetBooksInCartAsync(string username);
        Task<IdentityResult> UpdateBooksInCartAsync(string username, List<int> bookIds);
    }
}
