namespace PizzaManagementSystem.Models;

public class OrderDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public List<int> Items { get; set; } = [];
}