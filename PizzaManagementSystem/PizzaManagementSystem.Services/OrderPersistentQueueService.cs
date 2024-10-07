using DiskQueue;
using Microsoft.Extensions.Logging;
using PizzaManagementSystem.Models;

// ReSharper disable StringLiteralTypo

namespace PizzaManagementSystem.Services;

public class OrderPersistentQueueService(ILogger<OrderPersistentQueueService> logger) : IOrderService
{
    private readonly IPersistentQueue<Order> _orders = new PersistentQueue<Order>("OrdersQueue", 1 * 1024 * 1024);  // 1 MB

    public async Task QueueOrder(Order order)
    {
        await Task.Run(() =>
        {
            using var session = _orders.OpenSession();
            session.Enqueue(order);
            session.Flush();

            logger.LogInformation($"Added order id {order.Id} to Queue. Total order price is {order.TotalPrice:C}");
        });
    }

    public int GetPendingOrders()
    {
        return _orders.EstimatedCountOfItemsInQueue;
    }

    public Task<Order?> GetNextOrder()
    {
        using var session = _orders.OpenSession();
        var order = session.Dequeue();
        session.Flush();
        return Task.FromResult(order);
    }

    public List<Pizza> MenuPizza =>
    [
        new()
        {
            Id = 1,
            Name = "Margherita",
            Price = 5.0M
        },

        new()
        {
            Id = 2,
            Name = "Ortolana",
            Price = 6.0M
        },

        new()
        {
            Id = 3,
            Name = "Diavola",
            Price = 6.50M
        },

        new()
        {
            Id = 4,
            Name = "Bufalina",
            Price = 7.0M
        }
    ];
}