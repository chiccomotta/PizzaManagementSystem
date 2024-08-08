using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoreAPI.Mongo.Entity
{
    public class Books
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string category { get; set; }
        public string author { get; set; }
    }
}
