using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative")]
        public int Stock { get; set; }
    }

    public class UpdateProductDto
    {
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double? Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative")]
        public int? Stock { get; set; }
    }

    public class ProductResponseDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Stock { get; set; }
    }

}
