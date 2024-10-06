using BookWyrmAPI2.DataAccess.IRepository;
using BookWyrmAPI2.Models.DTOs.BundleDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookWyrmAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BundleController : ControllerBase
    {
        private readonly IBundleRepository _bundleRepository;

        public BundleController(IBundleRepository bundleRepository)
        {
            _bundleRepository = bundleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBundlesListAsync()
        {
            try
            {
                var bundles = await _bundleRepository.GetBundlesListAsync();
                return Ok(bundles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBundleByIdAsync(int id)
        {
            try
            {
                var bundle = await _bundleRepository.GetBundleByIdAsync(id);
                if (bundle == null) return NotFound();
                return Ok(bundle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("adminpanel/{id}")]
        public async Task<IActionResult> GetBundleWithBookListAsync(int id)
        {
            try
            {
                var bundle = await _bundleRepository.GetBundleWithBookListAsync(id);
                if (bundle == null) return NotFound();
                return Ok(bundle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBundleAsync([FromBody] BundleCreateDto bundleCreateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var bundle = await _bundleRepository.CreateBundleAsync(bundleCreateDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBundleAsync([FromBody] BundleUpdateDto bundleUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var updatedBundle = await _bundleRepository.UpdateBundleAsync(bundleUpdateDto);
                if (updatedBundle == null) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBundleAsync(int id)
        {
            try
            {
                var deletedBundle = await _bundleRepository.DeleteBundleAsync(id);
                if (deletedBundle == null) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
