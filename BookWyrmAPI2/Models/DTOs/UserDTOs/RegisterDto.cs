using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.DTOs.UserDTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters long.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long. It must contain an uppercase letter, a number and a non alphanumeric character.")]
        public string Password { get; set; }
    }
}
