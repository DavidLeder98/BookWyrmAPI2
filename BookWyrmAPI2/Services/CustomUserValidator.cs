using BookWyrmAPI2.Models.BaseModels;
using Microsoft.AspNetCore.Identity;

namespace BookWyrmAPI2.Services
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();

            // Checks username length
            if (user.UserName.Length > 20)
            {
                errors.Add(new IdentityError
                {
                    Code = "InvalidUserName",
                    Description = "The username must be at most 20 characters long."
                });
            }

            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
