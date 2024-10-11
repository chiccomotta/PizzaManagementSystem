using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using PizzaManagementSystem.Api.Middlewares;
using PizzaManagementSystem.Models.Extensions;
using PizzaManagementSystem.Models.Models;
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
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.AddSecurityDefinition("BearerScheme", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    cfg.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "BearerScheme"
                }
            },
            []      // no scopes
        }
    });
});

// Business services
builder.Services.AddSingleton<IOrderService, OrderPersistentQueueService>();
builder.Services.AddScoped<GlobalExceptionMiddleware>();
builder.Services.AddScoped<IUserContext, UserContext>();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(OrderDtoValidator).Assembly).AddFluentValidationAutoValidation();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateOrderCommand).Assembly));

// EF Core
builder.Services.AddDbPizzeContext(configuration);

// ASP Identity configuration endpoints
builder.Services
    .AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DBContext>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("/auth").MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();
