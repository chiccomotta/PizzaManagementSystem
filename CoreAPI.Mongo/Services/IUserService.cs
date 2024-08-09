using CoreAPI.Mongo.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreAPI.Mongo.Services;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByIdAsync(string id);
    Task<User> CreateAsync(User book);
    Task UpdateAsync(string id, User book);
    Task DeleteAsync(string id);
    Task<int> CreateAllAsync(List<User> books);
}