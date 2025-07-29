using DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{   public class Order:BaseEntity
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be a non-negative value.")]
        public double TotalAmount { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public OrderStatus Status { get; set; }

        // Navigation property
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual Invoice? Invoice { get; set; }

    }
}
