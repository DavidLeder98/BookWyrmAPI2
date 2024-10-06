using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

namespace BookWyrmAPI2.DataAccess.IRepository
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookListDto>> GetBookListAsync(string? searchTerm);
        Task<IEnumerable<BookCardDto>> GetBookCardsAsync(string? searchTerm, SortBy sortBy);
        Task<BookDetailsDto> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(BookCreateDto bookCreateDto);
        Task<Book> UpdateBookAsync(BookUpdateDto bookUpdateDto);
        Task<Book> DeleteBookAsync(int id);
    }
}
