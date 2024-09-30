namespace PizzaManagementSystem.Models;

public class Order
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public List<Pizza> Items { get; set; } = [];
    public decimal TotalPrice
    {
        get
        {
            return Items.Sum(p => p.Price);
        }
    }
}

public class PlacedOrder
{
    public Guid OrderId { get; init; }
    public decimal TotalPrice { get; init; }
    public int PendingOrders { get; init; }
}

