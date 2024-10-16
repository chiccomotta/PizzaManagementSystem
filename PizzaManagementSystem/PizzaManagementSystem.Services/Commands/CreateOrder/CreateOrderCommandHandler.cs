using MediatR;
using PizzaManagementSystem.Models;
using PizzaManagementSystem.Services.Interfaces;

namespace PizzaManagementSystem.Services.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(IOrderService orderService) : IRequestHandler<CreateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                Items = request.Items
            };

            await orderService.QueueOrder(order);
            return order.Id;
        }
    }
}
