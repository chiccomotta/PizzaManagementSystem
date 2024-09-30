using MediatR;
using PizzaManagementSystem.Models;

namespace PizzaManagementSystem.Services.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
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
}
