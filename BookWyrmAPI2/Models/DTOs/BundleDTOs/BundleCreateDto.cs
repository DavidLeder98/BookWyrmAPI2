namespace BookWyrmAPI2.Models.DTOs.BundleDTOs
{
    public class BundleCreateDto
    {
        public string Name { get; set; }
        public List<int> BookIds { get; set; } = new List<int>();
    }
}
