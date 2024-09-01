namespace CoreAPI.Mongo.Configuration;

public class MongoConfiguration
{
    public string BooksCollectionName { get; set; }
    public string UsersCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}