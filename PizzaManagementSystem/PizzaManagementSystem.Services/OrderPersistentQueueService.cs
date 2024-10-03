using DiskQueue;
using Microsoft.Extensions.Logging;
using PizzaManagementSystem.Models;

namespace PizzaManagementSystem.Services;

public class OrderPersistentQueueService(ILogger<OrderPersistentQueueService> logger) : IOrderService
{
    private IPersistentQueue<Order> Orders = new PersistentQueue<Order>("OrdersQueue");

    public async Task QueueOrder(Order order)
    {
        await Task.Run(() =>
        {
            using var session = Orders.OpenSession();
            session.Enqueue(order);
            session.Flush();

            logger.LogInformation($"Added order id {order.Id} to Queue. Total price is {order.TotalPrice:C}");
        });
    }

    public int GetPendingOrders()
    {
        return Orders.EstimatedCountOfItemsInQueue;
    }

    public Task<Order?> GetNextOrder()
    {
        using var session = Orders.OpenSession();
        var order = session.Dequeue();
        session.Flush();
        return Task.FromResult(order);
    }

    public List<Pizza> MenuPizza =>
    [
        new Pizza()
        {
            Id = 1,
            Name = "Margherita",
            Price = 5.0M
        },

        new Pizza()
        {
            Id = 2,
            Name = "Ortolana",
            Price = 6.0M
        },

        new Pizza()
        {
            Id = 3,
            Name = "Diavola",
            Price = 6.50M
        },

        new Pizza()
        {
            Id = 4,
            Name = "Bufalina",
            Price = 7.0M
        }
    ];
}