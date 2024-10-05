using System.ComponentModel.DataAnnotations;

namespace BookWyrmAPI2.Models.BaseModels
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Description { get; set; }

        // one to many, author to book
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
