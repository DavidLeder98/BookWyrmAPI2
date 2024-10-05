using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.BookDTOs;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

namespace BookWyrmAPI2.DataAccess.IRepository
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookListDto>> GetBookListAsync();
        Task<IEnumerable<BookCardDto>> GetBookCardsAsync(string? searchTerm, SortBy sortBy);
        Task<BookDetailsDto> GetBookByIdAsync(int id);
    }
}
