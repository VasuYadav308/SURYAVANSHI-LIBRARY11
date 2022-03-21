using System.ComponentModel.DataAnnotations;

namespace SURYAVANSHI_LIBRARY.Models
{
    public class Customer
    {
       
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        [StringLength(250)]
        public string Phone { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [StringLength(2050)]
        public bool IsDeleted { get; set; }

    }
}
