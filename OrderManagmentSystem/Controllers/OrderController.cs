using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces.Services;
using DAL.DTOs;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace OrderManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMailService _mailService;
        public OrderController(IOrderService orderService, IMailService mailService)
        {
            _orderService = orderService;
            _mailService = mailService;
        }

        // POST /api/orders
        [HttpPost]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> CreateOrder([FromQuery] int customerId, [FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(customerId, orderDto);
            if (order == null)
                return BadRequest("Order creation failed. Check stock and order details.");

            // TODO: Generate invoice and send email notification if needed
            return Ok(order);
        }

        // GET /api/orders/{orderId}
        [HttpGet("{orderId}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // GET /api/orders (admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // PUT /api/orders/{orderId}/status (admin only)
        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto statusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _orderService.UpdateOrderStatusAsync(orderId, statusDto.Status);
            if (!updated)
                return BadRequest("Failed to update order status.");

            // TODO: Send email notification to customer about status change
            return Ok("Order status updated.");
        }

        // Test endpoint for OrderItem creation
        [HttpPost("test-orderitem")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TestOrderItem(int orderId, int productId, int quantity)
        {
            var result = await _orderService.TestOrderItemCreation(orderId, productId, quantity);
            return Ok(new { Success = result });
        }
    }
} 