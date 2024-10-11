using BookWyrmAPI2.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookWyrmAPI2.Models.DTOs.CategoryDTOs;

namespace BookWyrmAPI2.Models.DTOs.BookDTOs
{
    public class BookDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public bool BestSeller { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Price { get; set; }
        public string? LargeImageUrl { get; set; }

        // Author properties
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }

        // Categories properties
        public List<CategoryListDto> Categories { get; set; } = new List<CategoryListDto>();
    }
}
