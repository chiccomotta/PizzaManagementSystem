using FluentValidation;
using FluentValidation.AspNetCore;
using PizzaManagementSystem.Api.Middlewares;
using PizzaManagementSystem.Models.Validators;
using PizzaManagementSystem.Services;
using PizzaManagementSystem.Services.Commands.CreateOrder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IOrderService>(new OrderService());
builder.Services.AddScoped<GlobalExceptionMiddleware>();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(OrderDtoValidator).Assembly).AddFluentValidationAutoValidation();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateOrderCommand).Assembly));

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
