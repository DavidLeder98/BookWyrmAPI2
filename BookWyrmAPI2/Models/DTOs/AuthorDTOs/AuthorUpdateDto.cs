using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.DTOs.AuthorDTOs
{
    public class AuthorUpdateDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Description { get; set; }
    }
}
