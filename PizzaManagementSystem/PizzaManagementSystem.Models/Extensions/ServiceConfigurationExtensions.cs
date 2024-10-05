using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaManagementSystem.Models.Models;

namespace PizzaManagementSystem.Models.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void AddDbPizzeContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DBContext>(options => options.UseSqlServer(configuration.GetConnectionString("DBPizze")));
    }
}