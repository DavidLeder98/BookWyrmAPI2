using BookWyrmAPI2.Models.DTOs.BookDTOs;

namespace BookWyrmAPI2.Models.DTOs.AuthorDTOs
{
    public class AuthorWithBooksDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BookCardDto> Books { get; set; } = new List<BookCardDto>();
    }
}
