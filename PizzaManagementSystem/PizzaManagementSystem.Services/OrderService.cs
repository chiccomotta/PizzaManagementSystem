using PizzaManagementSystem.Models;
using System.Collections.Concurrent;

namespace PizzaManagementSystem.Services;

public class OrderService : IOrderService
{
    public static ConcurrentQueue<Order> Orders = new();
   
    public async Task QueueOrder(Order order)
    {
        await Task.Run(() =>
        {
            Orders.Enqueue(order);
        });
    }

    public int GetPendingOrders()
    {
        return Orders.Count;
    }

    public Task<Order?> GetNextOrder()
    {
        var order = Orders.TryDequeue(out var nextOrder) ? nextOrder : null;
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