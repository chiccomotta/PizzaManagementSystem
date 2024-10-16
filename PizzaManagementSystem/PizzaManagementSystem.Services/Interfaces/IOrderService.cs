using PizzaManagementSystem.Models;

namespace PizzaManagementSystem.Services.Interfaces;

public interface IOrderService
{
    Task QueueOrder(Order order);
    int GetPendingOrders();
    Task<Order?> GetNextOrder();
    List<Pizza> MenuPizza { get; }
}