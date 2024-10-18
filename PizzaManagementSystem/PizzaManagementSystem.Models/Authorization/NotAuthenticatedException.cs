namespace PizzaManagementSystem.Models.Authorization
{
    public class NotAuthenticatedException(string message) : Exception(message);
}
