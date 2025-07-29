using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Invoice:BaseEntity
    {
        [Key]
        public int InvoiceID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be a non-negative value.")]
        public double TotalAmount { get; set; }
        // Navigation properties
        public virtual Order Order { get; set; } = null!;
    }
}
