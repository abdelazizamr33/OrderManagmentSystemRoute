using AutoMapper;
using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MappingProfiles
{
    public class InvoiceProfile:Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<Invoice, InvoiceResponseDto>().ReverseMap();
            CreateMap<CreateInvoiceDto, Invoice>().ReverseMap();
        }
    }
}
