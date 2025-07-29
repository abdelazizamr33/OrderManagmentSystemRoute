using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class OrderItem:BaseEntity
    {
        [Key]
        public int OrderItemId { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be a non-negative value.")]
        public double UnitPrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Discount must be a non-negative value.")]
        public double Discount { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; } 
    }
}
