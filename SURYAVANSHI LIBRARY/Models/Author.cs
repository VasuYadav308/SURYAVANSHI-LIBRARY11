using System.ComponentModel.DataAnnotations;

namespace SURYAVANSHI_LIBRARY.Models
{
    public class Author
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(2050)]
        public string Notes { get; set; }

    }
}
