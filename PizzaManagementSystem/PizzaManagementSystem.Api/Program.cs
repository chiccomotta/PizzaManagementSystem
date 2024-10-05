using FluentValidation;
using FluentValidation.AspNetCore;
using PizzaManagementSystem.Api.Middlewares;
using PizzaManagementSystem.Models.Extensions;
using PizzaManagementSystem.Models.Validators;
using PizzaManagementSystem.Services;
using PizzaManagementSystem.Services.Commands.CreateOrder;
using System.Text.Json.Serialization;
// ReSharper disable StringLiteralTypo

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    // Not Exclude null properties during serialization
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IOrderService, OrderPersistentQueueService>();
builder.Services.AddScoped<GlobalExceptionMiddleware>();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(OrderDtoValidator).Assembly).AddFluentValidationAutoValidation();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateOrderCommand).Assembly));

// EFCore
builder.Services.AddDbPizzeContext(configuration);
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
