using Microsoft.AspNetCore.Authorization;

namespace PizzaManagementSystem.Models.Authorization;

public class EmailRequirement(string domain) : IAuthorizationRequirement
{
    public string Domain { get; } = domain;
}