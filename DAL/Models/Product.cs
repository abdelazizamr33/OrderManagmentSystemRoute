using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Product:BaseEntity
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value.")]
        public double Price { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative value.")]
        public int Stock { get; set; }
        // nav prop
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
