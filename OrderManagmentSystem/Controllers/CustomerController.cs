using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using DAL.Models.Enums;

namespace OrderManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        public CustomerController(ICustomerService customerService, IOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
        }

        // POST /api/customers
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _customerService.CreateCustomerAsync(customerDto);
            if (customer == null)
                return BadRequest("Customer creation failed. Email may already exist.");

            return Ok(customer);
        }

        // GET /api/customers/{customerId}/orders
        [HttpGet("{customerId}/orders")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetCustomerOrders(int customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
            return Ok(orders);
        }
    }
} 