using BookWyrmAPI2.DataAccess.Repository;
using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.AuthorDTOs;
using static BookWyrmAPI2.DataAccess.Repository.BookRepository;

namespace BookWyrmAPI2.DataAccess.IRepository
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<AuthorListDto>> GetAuthorsListAsync(string? searchTerm);
        Task<AuthorWithBooksDto> GetAuthorByIdAsync(int id, SortBy sortBy);
        Task<Author> CreateAuthorAsync(AuthorCreateDto authorCreateDto);
        Task<Author> UpdateAuthorAsync(AuthorUpdateDto authorUpdateDto);
        Task<Author> DeleteAuthorAsync(int id);
    }
}
