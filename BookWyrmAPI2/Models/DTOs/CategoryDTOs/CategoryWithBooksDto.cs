using BookWyrmAPI2.Models.DTOs.BookDTOs;

namespace BookWyrmAPI2.Models.DTOs.CategoryDTOs
{
    public class CategoryWithBooksDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookCardDto> Books { get; set; } = new List<BookCardDto>();
    }
}
