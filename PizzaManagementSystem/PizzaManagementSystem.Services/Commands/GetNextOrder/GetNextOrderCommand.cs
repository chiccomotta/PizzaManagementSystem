using MediatR;
using PizzaManagementSystem.Models;

namespace PizzaManagementSystem.Services.Commands.GetNextOrder;

public class GetNextOrderCommand : IRequest<Order?>
{
}