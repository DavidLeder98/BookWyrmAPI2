using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

namespace BookWyrmAPI2.DataAccess.IRepository
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookListDto>> GetBookListAsync(string? searchTerm);
        Task<IEnumerable<BookCardDto>> GetBookCardsAsync(string? searchTerm, SortBy sortBy);
        Task<BookCardDto> GetBookCardByIdAsync(int id);
        Task<BookDetailsDto> GetBookByIdAsync(int id);
        Task<string> CreateBookAsync(BookCreateDto bookCreateDto);
        Task<string> UpdateBookAsync(BookUpdateDto bookUpdateDto);
        Task<string> DeleteBookAsync(int id);
    }
}
