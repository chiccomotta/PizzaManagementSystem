namespace PizzaManagementSystem.Models.Models;

public record CurrentUser(string UserId, string Email, string? Nickname, IEnumerable<string> Roles, IEnumerable<string>? ProgramminLanguages);