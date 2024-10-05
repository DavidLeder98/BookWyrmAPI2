using BookWyrmAPI2.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public string? ImageUrl { get; set; }

        // Author properties
        public int AuthorId { get; set; } // Include AuthorId
        public string AuthorName { get; set; } // Include AuthorName

        // Categories properties
        public List<int> CategoryIds { get; set; } = new List<int>(); // List of Category IDs
        public List<string> CategoryNames { get; set; } = new List<string>(); // List of Category Names
    }
}
