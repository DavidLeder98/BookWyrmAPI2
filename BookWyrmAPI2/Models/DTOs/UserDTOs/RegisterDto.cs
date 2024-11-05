using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.DTOs.UserDTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9._]+$", ErrorMessage = "Username can only contain letters, numbers, '.' and '_'")]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "Password must include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
    }
}
