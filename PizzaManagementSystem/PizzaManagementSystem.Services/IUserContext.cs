using PizzaManagementSystem.Models.Models;

namespace PizzaManagementSystem.Services;

public interface IUserContext
{
    Task<CurrentUser> GetCurrentUser();
}