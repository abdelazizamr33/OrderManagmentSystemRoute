using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Customer:BaseEntity
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        // Foreign key
        public int UserId { get; set; }
        // Nav prop
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual User? User { get; set; }
    }
}
