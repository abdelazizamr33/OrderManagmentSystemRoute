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
    public class OrderProfile: Profile
    {
        public OrderProfile()
        {
            // CreateMap<Order, OrderResponseDto>()
            //     .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            //     .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems.Select(oi => new OrderItemDto
            //     {
            //         ProductId = oi.ProductId,
            //         Quantity = oi.Quantity,
            //         UnitPrice = oi.UnitPrice,
            //         Discount = oi.Discount
            //     })));
            CreateMap<Order, OrderResponseDto>();
        }
    }
    
}
