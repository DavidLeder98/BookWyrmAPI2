namespace BookWyrmAPI2.Models.DTOs.BundleDTOs
{
    public class BundleUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> BookIds { get; set; } = new List<int>();
    }
}
