using Microsoft.AspNetCore.Identity;

namespace BookWyrmAPI2.Models.BaseModels
{
    public class AppUser : IdentityUser
    {
        public string? VerificationToken { get; set; }
        public bool IsVerified { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? TelNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public List <int>? BookIds { get; set; } = new List<int> ();
    }
}
