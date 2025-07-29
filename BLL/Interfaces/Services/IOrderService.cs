using DAL.DTOs;
using DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IOrderService
    {
        // create new order - GetOrderbyId(id) - GetAllOrders - UpdateOrderStatus(id) - 
        Task<OrderResponseDto?> CreateOrderAsync(int customerId, CreateOrderDto createOrderDto);
        Task<OrderResponseDto?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<double> CalculateOrderTotalAsync(List<OrderItemDto> orderItems);
        Task<double> ApplyDiscountAsync(double totalAmount);
    }
}
