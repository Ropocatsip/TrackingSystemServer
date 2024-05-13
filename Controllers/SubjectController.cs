using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class SubjectController : ControllerBase
{
    private readonly ILogger<SubjectController> _logger;
    IMongoCollection<Subject> collectionUser;
    
    public SubjectController(ILogger<SubjectController> logger)
    {
        _logger = logger;
    }

    private void GetConnectionString()
    {
        const string connectionUri = "mongodb://localhost:27017";
        var client = new MongoClient(connectionUri);
        var database = client.GetDatabase("tracking_system");
        collectionUser = database.GetCollection<Subject>("Subject");
    }

    [HttpGet("")]
    public async Task<IActionResult> GetSubjects()
    {
        GetConnectionString();
        List<Subject> courses = await collectionUser.Find(_ => true).ToListAsync();
        return Ok(courses);
    }

}