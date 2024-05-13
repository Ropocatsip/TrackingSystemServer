using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Models;

public class Advisor
{
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ObjectId { get; set; } = "";
    public int AdvisorId { get; set; }
    public string UserName { get; set; } = "";
    public List<AdvisoryRequest> AdvisoryRequest { get; set; } = new List<AdvisoryRequest>();
}