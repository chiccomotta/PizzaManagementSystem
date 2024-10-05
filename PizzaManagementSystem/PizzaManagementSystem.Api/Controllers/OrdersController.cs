using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaManagementSystem.Models;
using PizzaManagementSystem.Models.Models;
using PizzaManagementSystem.Services;
using PizzaManagementSystem.Services.Commands.CreateOrder;
using PizzaManagementSystem.Services.Commands.GetMenu;
using PizzaManagementSystem.Services.Commands.GetNextOrder;
// ReSharper disable All
// ReSharper disable InconsistentNaming

namespace PizzaManagementSystem.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService _orderService, IMediator _mediator, DBContext context) : ControllerBase
{
    private readonly IOrderService orderService = _orderService;
    private readonly IMediator mediator = _mediator;
    private readonly DBContext context = context;

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
        var order = await mediator.Send(new GetNextOrderCommand());
        if (order is not null)
        {
            return Ok(order);
        }

        return Ok("Nessun ordine da processare");
    }

    [HttpGet]
    [Route("area/{id:int}")]
    public async Task<ActionResult> GetArea(int id)
    {
        var area = await context.Areas
            .Where(a => a.AreaId == id)
            .FirstOrDefaultAsync();

        if (area is not null)
        {
            return Ok(area);
        }

        return NotFound();
    }
}