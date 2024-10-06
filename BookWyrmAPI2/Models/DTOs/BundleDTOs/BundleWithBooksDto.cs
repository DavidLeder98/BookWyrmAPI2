using BookWyrmAPI2.Models.DTOs.BookDTOs;

namespace BookWyrmAPI2.Models.DTOs.BundleDTOs
{
    public class BundleWithBooksDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookCardDto> Books { get; set; } = new List<BookCardDto>();
    }
}
