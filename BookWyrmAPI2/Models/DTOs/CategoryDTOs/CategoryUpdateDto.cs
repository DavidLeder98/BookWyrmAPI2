using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.DTOs.CategoryDTOs
{
    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30), MinLength(1)]
        public string Name { get; set; }
    }
}
