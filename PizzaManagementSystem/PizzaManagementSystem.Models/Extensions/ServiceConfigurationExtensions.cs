using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaManagementSystem.Models.Models;

namespace PizzaManagementSystem.Models.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void AddDBPizzeContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DBPizze");
        services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString));
    }
}