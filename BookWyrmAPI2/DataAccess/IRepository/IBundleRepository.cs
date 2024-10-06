using BookWyrmAPI2.Models.BaseModels;
using BookWyrmAPI2.Models.DTOs.AuthorDTOs;
using BookWyrmAPI2.Models.DTOs.BundleDTOs;

namespace BookWyrmAPI2.DataAccess.IRepository
{
    public interface IBundleRepository
    {
        Task<IEnumerable<BundleListDto>> GetBundlesListAsync();
        Task<BundleWithBooksDto> GetBundleByIdAsync(int id); //for users to view books in bundle
        Task<BundleWithBookListDto> GetBundleWithBookListAsync(int id); //for admins to view in the content managment panel
        Task<Bundle> CreateBundleAsync(BundleCreateDto bundleCreateDto);
        Task<Bundle> UpdateBundleAsync(BundleUpdateDto bundleUpdateDto);
        Task<Bundle> DeleteBundleAsync(int id);
    }
}
