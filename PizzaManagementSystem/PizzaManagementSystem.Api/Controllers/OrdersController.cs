using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaManagementSystem.Models;
using PizzaManagementSystem.Services;
using PizzaManagementSystem.Services.Commands.CreateOrder;
using PizzaManagementSystem.Services.Commands.GetMenu;
// ReSharper disable InconsistentNaming

namespace PizzaManagementSystem.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService _orderService, IMediator _mediator) : ControllerBase
{
    public readonly IOrderService orderService = _orderService;
    public readonly IMediator mediator = _mediator;


    [HttpGet]
    [Route("Menu")]
    public async Task<ActionResult<List<Pizza>>> Menu()
    {
        var menu = await mediator.Send(new GetMenuCommand());
        return Ok(menu);
    }

    [HttpPost]
    [Route("AddOrder")]
    public async Task<ActionResult<Order>> AddOrder([FromBody] OrderDto request)
    {
        // Sarebbe meglio utilizzare FluentValidation
        if (request.Items.Count == 0)
        {
            return BadRequest("Selezionare almeno una pizza dal menu");
        }

        // Id delle pizze
        if (!request.Items.All(id => id is >= 1 and <= 4))
        {
            return BadRequest("L'Id della pizza deve essere >= 1 e <= 4");
        }

        // Leggo i pending orders
        var pendingOrders = orderService.GetPendingOrders();

        var command = new CreateOrderCommand()
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            Items = request.Items.Select(i => orderService.MenuPizza.FirstOrDefault(p => p.Id == i)).ToList()!
        };

        // Accodo l'ordine
        await mediator.Send(command);

        return Ok(new PlacedOrder()
        {
            OrderId = command.Id,
            TotalPrice = command.TotalPrice,
            PendingOrders = pendingOrders
        });
    }

    [HttpGet]
    [Route("GetNextOrder")]
    public async Task<ActionResult<Order>> GetNextOrder()
    {
        var order = orderService.GetNextOrder();
        if (order is not null)
        {
            return Ok(order);
        }

        return Ok("Nessun ordine da processare");
    }
}