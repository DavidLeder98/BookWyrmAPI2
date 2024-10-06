using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.BaseModels
{
    public class Bundle
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
        // many to many with book
        public ICollection<BookBundle> BookBundles { get; set; } = new List<BookBundle>();
    }
}
