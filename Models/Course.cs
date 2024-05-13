using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Models;

[BsonIgnoreExtraElements]
public class Course
{
    // [BsonElement("_id")]
    // [BsonRepresentation(BsonType.ObjectId)]
    // public string ObjectId { get; set; } = "";
    public string CourseId { get; set; } = "";
    public string Desc { get; set; } = "";
    public List<Subject> Subject { get; set; } = [];
}
