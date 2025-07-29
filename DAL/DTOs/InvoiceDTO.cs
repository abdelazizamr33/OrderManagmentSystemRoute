using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class InvoiceDto
    {
        public int InvoiceID { get; set; }
        public int OrderID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalAmount { get; set; }
    }

    public class CreateInvoiceDto
    {
        public int OrderID { get; set; }
        public double TotalAmount { get; set; }
    }

    
    public class InvoiceResponseDto
    {
        public int InvoiceID { get; set; }
        public int OrderID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalAmount { get; set; }
    }
}
