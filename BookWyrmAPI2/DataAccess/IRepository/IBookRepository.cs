using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.BookDTOs;

namespace BookWyrmAPI2.DataAccess.IRepository
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookListDto>> GetBookListAsync();
        Task<IEnumerable<BookListDto>> GetBookListAlphabeticallyAsync();
        Task<IEnumerable<BookCardDto>> GetBookCardsAsync();
    }
}
