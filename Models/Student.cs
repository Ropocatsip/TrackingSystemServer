using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Models;

public class Student
{
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ObjectId { get; set; } = "";
    public string StudentId { get; set; } = "";
    public Course Course { get; set; } = new Course();
    public decimal GPAX { get; set; } = 0;
    public StudentStatus Status { get; set; } = new StudentStatus();
    public string UserName { get; set; } = "";
    public Thesis Thesis { get; set; } = new Thesis();
    
}
