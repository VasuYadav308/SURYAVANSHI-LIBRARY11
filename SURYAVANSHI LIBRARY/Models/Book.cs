using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SURYAVANSHI_LIBRARY.Models
{
    public class Book
    {
        [Key]
        [StringLength(25)]
        public string ISBN { get; set; }
        [StringLength(250)]
        public string Title { get; set; }
        public int PublisherId { get; set; }
        public int AuthorId { get; set; }
        public bool IssuedStatus { get; set; }
        public bool IsDeleted { get; set; }


        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

    }
}
