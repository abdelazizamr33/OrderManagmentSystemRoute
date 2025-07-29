using AutoMapper;
using BLL.Interfaces;
using BLL.Interfaces.Services;
using BLL.Repositories;
using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class InvoiceService(IUnitOfWork unitOfWork,IMapper mapper) : IInvoiceService
    {
        public async Task<InvoiceResponseDto?> CreateInvoiceAsync(CreateInvoiceDto dto)
        {
            var invoice = new Invoice
            {
                OrderID = dto.OrderID,
                TotalAmount = dto.TotalAmount,
                InvoiceDate = DateTime.UtcNow
            };

            await unitOfWork.GetRepository<Invoice>().AddAsync(invoice);
            var createdInvoice =unitOfWork.GetRepository<Invoice>().GetByIdAsync(invoice.InvoiceID);
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<InvoiceResponseDto>(createdInvoice);
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync()
        {
            var invoices = await unitOfWork.GetRepository<Invoice>().GetAllAsync(false);
            return mapper.Map<IEnumerable<InvoiceResponseDto>>(invoices);
        }

        public async Task<InvoiceResponseDto?> GetInvoiceByIdAsync(int id)
        {
            var invoice = await unitOfWork.GetRepository<Invoice>().GetByIdAsync(id);
            return invoice != null ? mapper.Map<InvoiceResponseDto>(invoice) : null;
        }
    }
}
