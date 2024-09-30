using MediatR;
using PizzaManagementSystem.Models;

namespace PizzaManagementSystem.Services.Commands.GetNextOrder
{
    public class GetNextOrderCommandHandler(IOrderService orderService) : IRequestHandler<GetNextOrderCommand, Order?>
    {
        public async Task<Order?> Handle(GetNextOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderService.GetNextOrder();
            return order;
        }
    }
}
