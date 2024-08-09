using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CoreAPI.Mongo.Entity;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> Languages { get; set; }
    public Address Address{ get; set; }
    public int YearsProgramming { get; set; }
    public DateTime BirthDate { get; set; }
}