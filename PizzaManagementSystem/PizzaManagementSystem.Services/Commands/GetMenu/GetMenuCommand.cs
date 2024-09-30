using MediatR;
using PizzaManagementSystem.Models;

namespace PizzaManagementSystem.Services.Commands.GetMenu;

public class GetMenuCommand : IRequest<List<Pizza>>
{
}