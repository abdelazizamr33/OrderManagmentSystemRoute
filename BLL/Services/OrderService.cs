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

        // Save OrderItems and updated products
        await unitOfWork.SaveChangesAsync();

        // Verify OrderItems were saved
        var savedOrderItems = await unitOfWork.GetRepository<OrderItem>().GetAllAsync(false);
        var orderItemsForThisOrder = savedOrderItems.OfType<OrderItem>().Where(oi => oi.OrderID == order.OrderID);
        Console.WriteLine($"Verified OrderItems in database for Order {order.OrderID}: {orderItemsForThisOrder.Count()} items");

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

        // Validate status transition
        if (!IsValidStatusTransition(order.Status, status))
        {
            throw new InvalidOperationException($"Invalid status transition from {order.Status} to {status}");
        }

        order.Status = status;
        unitOfWork.GetRepository<Order>().Update(order);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
    {
        return newStatus switch
        {
            OrderStatus.Pending => currentStatus == OrderStatus.Pending, // Can only stay pending
            OrderStatus.Processing => currentStatus == OrderStatus.Pending,
            OrderStatus.Shipped => currentStatus == OrderStatus.Processing,
            OrderStatus.Delivered => currentStatus == OrderStatus.Shipped,
            OrderStatus.Completed => currentStatus == OrderStatus.Delivered,
            OrderStatus.Cancelled => currentStatus == OrderStatus.Pending || currentStatus == OrderStatus.Processing,
            _ => false
        };
    }

    public async Task<bool> CompleteOrderAsync(int orderId)
    {
        var order = await unitOfWork.GetRepository<Order>().GetByIdAsync(orderId);
        if (order == null) return false;

        // Check if order can be completed
        if (order.Status != OrderStatus.Delivered)
        {
            throw new InvalidOperationException($"Order must be in 'Delivered' status to be completed. Current status: {order.Status}");
        }

        order.Status = OrderStatus.Completed;
        unitOfWork.GetRepository<Order>().Update(order);
        await unitOfWork.SaveChangesAsync();
        
        // Note: OrderItems are kept for historical records
        // They should never be deleted as they represent the actual order history
        
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

    public async Task<bool> TestOrderItemCreation(int orderId, int productId, int quantity)
    {
        try
        {
            var orderItem = new OrderItem
            {
                OrderID = orderId,
                ProductID = productId,
                Quantity = quantity,
                UnitPrice = 10.0,
                Discount = 0
            };

            Console.WriteLine($"Testing OrderItem creation: OrderID={orderItem.OrderID}, ProductID={orderItem.ProductID}");
            
            await unitOfWork.GetRepository<OrderItem>().AddAsync(orderItem);
            await unitOfWork.SaveChangesAsync();
            
            Console.WriteLine("Test OrderItem saved successfully");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating test OrderItem: {ex.Message}");
            return false;
        }
    }
}