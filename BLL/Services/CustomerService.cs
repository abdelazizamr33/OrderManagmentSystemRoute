using AutoMapper;
using BLL.Interfaces;
using BLL.Interfaces.Services;
using BLL.Repositories;
using DAL.Data.Contexts;
using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CustomerService(IUnitOfWork unitOfWork,IMapper mapper) : ICustomerService
    {
        
        public async Task<CustomerResponseDto?> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            // check username,email
            var existingCustomer = await GetCustomerByEmailAsync(createCustomerDto.Email);
            if (existingCustomer != null)
            {
                throw new InvalidOperationException($"Customer with email {createCustomerDto.Email} already exists");
            }

            var customer = new Customer
            {
                Name = createCustomerDto.Name,
                Email = createCustomerDto.Email
            };

            await unitOfWork.GetRepository<Customer>().AddAsync(customer);
            await unitOfWork.SaveChangesAsync();

            return await GetCustomerByIdAsync(customer.CustomerId);

        }

        

        public async Task<IEnumerable<CustomerResponseDto>> GetAllCustomersAsync()
        {
            var customer= await unitOfWork.GetRepository<Customer>().GetAllAsync(false);
            return mapper.Map<IEnumerable<CustomerResponseDto>>(customer);
        }

        public async Task<CustomerResponseDto?> GetCustomerByEmailAsync(string email)
        {
            var customers =await  unitOfWork.GetRepository<Customer>().GetAllAsync(false);
            var customer = customers.OfType<Customer>().FirstOrDefault(c => c.Email == email);
            return customer != null ? mapper.Map<CustomerResponseDto>(customer) : null;
        }

        public async Task<CustomerResponseDto?> GetCustomerByIdAsync(int customerId)
        {
            var customer=await unitOfWork.GetRepository<Customer>().GetByIdAsync(customerId);
            return mapper.Map<CustomerResponseDto>(customer);
        }

        public async Task UpdateCustomerAsync(int customerId, UpdateCustomerDto updateCustomerDto)
        {
            var customer = await unitOfWork.GetRepository<Customer>().GetByIdAsync(customerId);
            if (customer == null) return;

            // Check if email is being updated and if it already exists
            if (!string.IsNullOrEmpty(updateCustomerDto.Email) && updateCustomerDto.Email != customer.Email)
            {
                var existingCustomer = await GetCustomerByEmailAsync(updateCustomerDto.Email);
                if (existingCustomer != null)
                {
                    throw new InvalidOperationException($"Customer with email {updateCustomerDto.Email} already exists");
                }
                customer.Email = updateCustomerDto.Email;
            }

            if (!string.IsNullOrEmpty(updateCustomerDto.Name))
            {
                customer.Name = updateCustomerDto.Name;
            }

            unitOfWork.GetRepository<Customer>().Update(customer);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await unitOfWork.GetRepository<Customer>().GetByIdAsync(customerId);
            if (customer == null) return;
            unitOfWork.GetRepository<Customer>().Delete(customer);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
