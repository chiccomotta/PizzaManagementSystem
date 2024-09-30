using PizzaManagementSystem.Models;

namespace PizzaManagementSystem.Services;

public interface IOrderService
{
    Task QueueOrder(Order order);
    int GetPendingOrders();
    Task<Order?> GetNextOrder();
    List<Pizza> MenuPizza { get;}
}