using CoreAPI.Mongo.Configuration;
using CoreAPI.Mongo.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreAPI.Mongo.Services;

public class UserService: IUserService
{
    private readonly IMongoCollection<User> _user;
    private readonly MongoConfiguration _settings;

    public UserService(IOptions<MongoConfiguration> settings)
    {
        _settings = settings.Value;
        var client = new MongoClient(_settings.ConnectionString);
        var database = client.GetDatabase(_settings.DatabaseName);
        _user = database.GetCollection<User>(_settings.UsersCollectionName);
    }
    public async Task<List<User>> GetAllAsync()
    {
        return await _user.Find(c => true).ToListAsync();
    }
    public async Task<User> GetByIdAsync(string id)
    {
        return await _user.Find(c => c.Id == id).FirstOrDefaultAsync();
    }
    public async Task<User> CreateAsync(User book)
    {
        await _user.InsertOneAsync(book);
        return book;
    }
    public async Task UpdateAsync(string id, User book)
    {
        await _user.ReplaceOneAsync(c => c.Id == id, book);
    }
    public async Task DeleteAsync(string id)
    {
        await _user.DeleteOneAsync(c => c.Id == id);
    }

    public async Task<int> CreateAllAsync(List<User> Users)
    {
        await _user.InsertManyAsync(Users);
        return Users.Count;
    }

    public async Task<List<User>> SearchByName(string firstname)
    {
        // Create a filter builder for the 'User' class
        var filterBuilder = Builders<User>.Filter;

        // Define the filter for 'FirstName'
        var firstName = firstname;
        var filter = filterBuilder.Eq(u => u.FirstName, firstName);
        
        // Execute the query
        var users = await _user.Find(filter).ToListAsync();

        return users;
    }
}