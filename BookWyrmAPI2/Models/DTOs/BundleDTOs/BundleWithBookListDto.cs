using BookWyrmAPI2.Models.DTOs.BookDTOs;

namespace BookWyrmAPI2.Models.DTOs.BundleDTOs
{
    public class BundleWithBookListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookListDto> Books { get; set; } = new List<BookListDto>();
    }
}
