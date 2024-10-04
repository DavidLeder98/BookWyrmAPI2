using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.BaseModels
{
    public class Book
    {
        [Key]
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

        [Url]
        public string? ImageUrl { get; set; }
    }
}
