using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Models;

public class UserInfo
{
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ObjectId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Name { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public User User { get; set; } = new User();
    public string Role { get; set; } = "";
}

public class User 
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}