using Bogus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaManagementSystem.Models;
using PizzaManagementSystem.Models.Authorization;
using PizzaManagementSystem.Models.Models;
using PizzaManagementSystem.Services;
using PizzaManagementSystem.Services.Commands.CreateOrder;
using PizzaManagementSystem.Services.Commands.GetMenu;
using PizzaManagementSystem.Services.Commands.GetNextOrder;
using Area = PizzaManagementSystem.Models.Models.Area;
using DBContext = PizzaManagementSystem.Models.Models.DBContext;

// ReSharper disable All
// ReSharper disable InconsistentNaming

namespace PizzaManagementSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService orderService, IMediator mediator, DBContext context, IUserContext userContext) : ControllerBase
{
    [HttpGet]
    [Route("Menu")]
    public async Task<ActionResult<List<Pizza>>> Menu()
    {
        var user = await userContext.GetCurrentUser();

        var menu = await mediator.Send(new GetMenuCommand());
        return Ok(menu);
    }

    [AllowAnonymous]
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
    [Authorize(Roles = UserRoles.Admin, Policy = Policies.HasNickname)]
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
    [Route("area/{id:int?}")]
    public async Task<ActionResult> GetArea(int? id)
    {
        if (id.HasValue)
        {
            var area = await context.Areas.Where(a => a.AreaId == id).FirstOrDefaultAsync();

            if (area is not null)
            {
                return Ok(area);
            }

            return NotFound();
        }
        else
        {
            return  Ok(await context.Areas.ToListAsync());
        }
    }

    [HttpGet]
    [Route("/area/Feed")]
    public async Task<ActionResult> FeedDb()
    {
        var faker = new Faker<Area>()
            .RuleFor(t => t.Description, f => f.Commerce.ProductDescription())
            .RuleFor(t => t.Codice, f => f.Random.Int(1, 700))
            .RuleFor(t => t.Name, f => f.Commerce.ProductName())
            .RuleFor(t => t.InsertDate, f => f.Date.Past())
            .RuleFor(t => t.InsertBy, f => f.Name.FullName())
            .RuleFor(t => t.ModifiedDate, f => f.Date.Past())
            .RuleFor(t => t.ModifiedBy, f => f.Name.FullName())
            .RuleFor(t => t.Enabled, f => f.Random.Bool());

        var areas = faker.Generate(10);

        var employees = new Faker<Impiegato>()
            .RuleFor(t => t.Firstname, f => f.Name.FirstName())
            .RuleFor(t => t.Surname, f => f.Name.LastName())
            .RuleFor(t => t.AreaId, f => f.Random.Int(1, 10))
            .RuleFor(t => t.Enabled, f => f.Random.Bool());

        var emps = employees.Generate(100);

        await context.Database.ExecuteSqlRawAsync("truncate table Impiegato; delete area; DBCC CHECKIDENT ('Area', RESEED, 0);");

        await context.Areas.AddRangeAsync(areas);
        await context.Impiegatos.AddRangeAsync(emps);

        await context.SaveChangesAsync();
        return Ok("DB populated");
    }

    [HttpGet()]
    [Route("/area/insert")]
    public async Task<ActionResult> TestInsert()
    {
        var area = new Area()
        {
            Name = "TestArea",
            Description = "Area description for test",
            Codice = 999,
            Enabled = true,
            Impiegatos = new List<Impiegato>()
            {
                new Impiegato()
                {
                    Firstname = "Gino",
                    Surname = "Landi",
                },
                new Impiegato()
                {
                    Firstname = "Pasquale",
                    Surname = "Fuzzi",
                },
            }
        };

        context.Areas.Add(area);
        await context.SaveChangesAsync();

        return Ok("OK Inserted");
    }
}