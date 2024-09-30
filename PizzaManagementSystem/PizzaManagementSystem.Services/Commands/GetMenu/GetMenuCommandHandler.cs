using MediatR;
using PizzaManagementSystem.Models;

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
