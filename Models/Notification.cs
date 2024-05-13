using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Models;

[BsonIgnoreExtraElements]
public class Notification
{
    // [BsonElement("_id")]
    // [BsonRepresentation(BsonType.ObjectId)]
    // public string ObjectId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Type { get; set; } = "";
    public string Detail { get; set; } = "";
    public DateTime SendDate { get; set; }
    public bool IsClose { get; set; } = false;
}