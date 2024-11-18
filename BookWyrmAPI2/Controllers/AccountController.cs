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
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IAccountRepository accountRepository, UserManager<AppUser> userManager)
        {
            _accountRepository = accountRepository;
            _userManager = userManager;
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
                    var user = await _userManager.FindByNameAsync(model.Username);
                    var roles = await _userManager.GetRolesAsync(user);

                    return Ok(new
                    {
                        Message = "Login successful",
                        Role = roles.FirstOrDefault() // Gets the first role or null if none
                    });
                }

                return Unauthorized("Invalid login attempt");
            }
            catch (Exception ex)
            {
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-profile/{username}")]
        public async Task<IActionResult> UpdateUserProfile(string username, [FromBody] UserProfileDto userProfileDto)
        {
            try
            {
                var user = await _accountRepository.FindUserByUsernameAsync(username);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

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
                var bookIds = await _accountRepository.GetBooksInCartAsync(username);
                if (bookIds == null || !bookIds.Any())
                {
                    return NotFound();
                }

                return Ok(bookIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{username}/bookIds")]
        public async Task<ActionResult> UpdateBooksInCart(string username, [FromBody] List<int> bookIds)
        {
            if (bookIds == null)
            {
                return BadRequest("Book IDs list is null.");
            }

            try
            {
                var result = await _accountRepository.UpdateBooksInCartAsync(username, bookIds);
                if (result.Succeeded)
                {
                    return NoContent();
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}