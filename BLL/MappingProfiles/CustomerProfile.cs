using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MappingProfiles
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile()
        {
            CreateMap<DAL.Models.Customer, DAL.DTOs.CustomerResponseDto>().ReverseMap();
            CreateMap<DAL.Models.Customer, DAL.DTOs.CreateCustomerDto>().ReverseMap();
            CreateMap<DAL.Models.Customer, DAL.DTOs.UpdateCustomerDto>().ReverseMap();
        }
    }
}
