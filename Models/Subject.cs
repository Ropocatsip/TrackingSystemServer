using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Models;

[BsonIgnoreExtraElements]
public class Subject
{
    public int SubjectId { get; set; }
    public string Name { get; set; } = "";
    public string Schedule { get; set; } = "";
    public string Grade { get; set; } = "";
}