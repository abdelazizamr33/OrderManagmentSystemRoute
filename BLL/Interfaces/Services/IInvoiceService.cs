using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceResponseDto?> CreateInvoiceAsync(CreateInvoiceDto dto);
        Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync();
        Task<InvoiceResponseDto?> GetInvoiceByIdAsync(int id);
    }
}
