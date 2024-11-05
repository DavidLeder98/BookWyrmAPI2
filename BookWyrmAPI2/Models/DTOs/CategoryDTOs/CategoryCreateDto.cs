using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.DTOs.CategoryDTOs
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(20), MinLength(1)]
        public string Name { get; set; }
    }
}
