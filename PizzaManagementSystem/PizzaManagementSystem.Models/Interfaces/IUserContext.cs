using PizzaManagementSystem.Models.Models;

namespace PizzaManagementSystem.Models.Interfaces;

public interface IUserContext
{
    Task<CurrentUser> GetCurrentUser();
}