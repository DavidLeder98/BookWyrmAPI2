using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookWyrmAPI2.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _accountRepository.RegisterAsync(model);
                if (result.Succeeded)
                {
                    return Ok("Registration successful");
                }

                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { errors });
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await _accountRepository.LoginAsync(model);
                if (result.Succeeded)
                {
                    return Ok("Login successful");
                }

                return Unauthorized("Invalid login attempt");
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountRepository.LogoutAsync();
                return Ok("Logout successful");
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserProfile(string username)
        {
            try
            {
                var userProfile = await _accountRepository.GetUserProfileAsync(username);
                if (userProfile == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-profile/{username}")]
        public async Task<IActionResult> UpdateUserProfile(string username, [FromBody] UserProfileDto userProfileDto)
        {
            try
            {
                // Use the repository method to find the user by username
                var user = await _accountRepository.FindUserByUsernameAsync(username);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Call the repository method to update the user profile
                var result = await _accountRepository.UpdateUserProfileAsync(username, userProfileDto);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return BadRequest(new { errors });
                }

                return Ok("Profile updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{username}/bookIds")]
        public async Task<ActionResult<List<int>>> GetBooksInCart(string username)
        {
            try
            {
                var bookIds = await _accountRepository.GetBooksInCartAsync(username); // Call your method to get book IDs
                if (bookIds == null || !bookIds.Any()) // Check if the list is null or empty
                {
                    return NotFound(); // Return 404 if no book IDs are found
                }

                return Ok(bookIds); // Return the list of book IDs with a 200 OK status
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 on error
            }
        }

        [HttpPut("{username}/bookIds")]
        public async Task<ActionResult> UpdateBooksInCart(string username, [FromBody] List<int> bookIds)
        {
            if (bookIds == null)
            {
                return BadRequest("Book IDs list is null."); // Handle null input
            }

            try
            {
                var result = await _accountRepository.UpdateBooksInCartAsync(username, bookIds);
                if (result.Succeeded)
                {
                    return NoContent(); // Return 204 No Content on success
                }

                return BadRequest(result.Errors); // Return errors if failed
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Handle exception
            }
        }
    }
}