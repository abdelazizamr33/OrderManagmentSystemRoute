using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto?> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task<CustomerResponseDto?> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<CustomerResponseDto>> GetAllCustomersAsync();
        Task UpdateCustomerAsync(int customerId, UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(int customerId);
        Task<CustomerResponseDto?> GetCustomerByEmailAsync(string email);
    }
}
