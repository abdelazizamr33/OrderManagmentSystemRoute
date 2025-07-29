using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class CreateCustomerDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class UpdateCustomerDto
    {
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }

    public class CustomerResponseDto
    {
        public int CustomerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public int OrderCount { get; set; }
        //public DateTime CreatedDate { get; set; }
    }
}
