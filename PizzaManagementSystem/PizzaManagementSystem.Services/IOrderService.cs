using PizzaManagementSystem.Models;

namespace PizzaManagementSystem.Services;

public interface IOrderService
{
    Task QueueOrder(Order order);
    int GetPendingOrders();
    Order GetNextOrder();
    List<Pizza> MenuPizza { get;}
}