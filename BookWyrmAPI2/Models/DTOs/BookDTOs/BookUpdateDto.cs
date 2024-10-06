using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.DTOs.BookDTOs
{
    public class BookUpdateDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Description { get; set; }
        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }
        [Required]
        public bool BestSeller { get; set; }
        [Required]
        [Range(1, 10000)]
        public decimal ListPrice { get; set; }
        [Required]
        [Range(1, 10000)]
        public decimal Price { get; set; }
        public IFormFile ImageFile { get; set; }

        // Author properties
        public int AuthorId { get; set; }
        // Categories properties
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
