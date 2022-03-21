using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace SURYAVANSHI_LIBRARY.Models
{
    public class Transaction
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Book Title")]
        public string BookId { get; set; }
        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; }


        [DataType(DataType.Date)]
        public DateTime DateOfIssue { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfReturn { get; set; }


        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

    }
}
