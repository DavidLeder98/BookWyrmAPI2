using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.BaseModels
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }
        // many to many with book
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    }
}
