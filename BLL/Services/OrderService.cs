using BLL.Interfaces.Services;
using DAL.Data.Contexts;
using DAL.DTOs;
using DAL.Models.Enums;
using DAL.Models;
using BLL.Interfaces;
using BLL.Repositories;
using AutoMapper;

public class OrderService(IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
{

    public async Task<OrderResponseDto?> CreateOrderAsync(int customerId, CreateOrderDto createOrderDto)
    {
        // Validate customer exists
        var customer = await unitOfWork.GetRepository<Customer>().GetByIdAsync(customerId);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {customerId} not found");
        }

        // Validate order items and check stock
        var orderItems = new List<OrderItem>();
        foreach (var item in createOrderDto.OrderItems)
        {
            var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(item.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {item.ProductId} not found");
            }

            if (product.Stock < item.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {item.Quantity}");
            }
        }

        // Calculate total and apply discount
        var subtotal = await CalculateOrderTotalAsync(createOrderDto.OrderItems);
        var totalAmount = await ApplyDiscountAsync(subtotal);

        // Create order
        var order = new Order
        {
            CustomerID = customerId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            PaymentMethod = createOrderDto.PaymentMethod,
            Status = OrderStatus.Pending
        };

        await unitOfWork.GetRepository<Order>().AddAsync(order);
        await unitOfWork.SaveChangesAsync(); // Save to get the OrderID

        // Create order items and update stock
        foreach (var itemDto in createOrderDto.OrderItems)
        {
            var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(itemDto.ProductId);
            if (product == null) continue;

            var orderItem = new OrderItem
            {
                OrderID = order.OrderID,
                ProductID = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price,
                Discount = 0 // Individual item discounts can be added here
            };

            await unitOfWork.GetRepository<OrderItem>().AddAsync(orderItem);

            // Update product stock
            product.Stock -= itemDto.Quantity;
            unitOfWork.GetRepository<Product>().Update(product);
        }

        // Generate invoice
        var invoice = new Invoice
        {
            OrderID = order.OrderID,
            InvoiceDate = DateTime.UtcNow,
            TotalAmount = totalAmount
        };

        await unitOfWork.GetRepository<Invoice>().AddAsync(invoice);
        await unitOfWork.SaveChangesAsync();

        return await GetOrderByIdAsync(order.OrderID);
    }

    public async Task<OrderResponseDto?> GetOrderByIdAsync(int orderId)
    {
        var order = await unitOfWork.GetRepository<DAL.Models.Order>().GetByIdAsync(orderId);
        if (order == null) return null;

        return mapper.Map<OrderResponseDto>(order);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerAsync(int customerId)
    {
        var orders = await unitOfWork.GetRepository<Order>().GetAllAsync(false);
        var customerOrders = orders.OfType<Order>().Where(o => o.CustomerID == customerId);
        return mapper.Map<IEnumerable<OrderResponseDto>>(orders);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
    {
        var orders = await unitOfWork.GetRepository<Order>().GetAllAsync(false);
        return mapper.Map<IEnumerable<OrderResponseDto>>(orders);
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await unitOfWork.GetRepository<Order>().GetByIdAsync(orderId);
        if (order == null) return false;

        order.Status = status;
        unitOfWork.GetRepository<Order>().Update(order);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<double> CalculateOrderTotalAsync(List<OrderItemDto> orderItems)
    {
        double total = 0;
        foreach (var item in orderItems)
        {
            var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(item.ProductId);
            if (product != null)
            {
                total += product.Price * item.Quantity;
            }
        }
        return total;
    }

    public async Task<double> ApplyDiscountAsync(double totalAmount)
    {
        if (totalAmount >= 200)
        {
            return totalAmount * 0.90; // 10% discount
        }
        else if (totalAmount >= 100)
        {
            return totalAmount * 0.95; // 5% discount
        }
        return totalAmount; // No discount
    }

    
}