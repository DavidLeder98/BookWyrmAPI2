namespace BookWyrmAPI2.Models.DTOs.BookDTOs
{
    public class BookCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public bool BestSeller { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? AuthorName { get; set; }
    }
}
