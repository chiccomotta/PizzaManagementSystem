using MediatR;
using PizzaManagementSystem.Models;
using PizzaManagementSystem.Services.Interfaces;

namespace PizzaManagementSystem.Services.Commands.GetMenu
{
    public class GetMenuCommandHandler(IOrderService orderService) : IRequestHandler<GetMenuCommand, List<Pizza>>
    {
        public Task<List<Pizza>> Handle(GetMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = orderService.MenuPizza;
            return Task.FromResult(menu);
        }
    }
}
