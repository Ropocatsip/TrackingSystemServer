using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using server.Models;
using Newtonsoft.Json;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;
    IMongoCollection<Student> collectionUser;
    
    public StudentController(ILogger<StudentController> logger)
    {
        _logger = logger;
    }

    private void GetConnectionString()
    {
        const string connectionUri = "mongodb://localhost:27017";
        var client = new MongoClient(connectionUri);
        var database = client.GetDatabase("tracking_system");
        collectionUser = database.GetCollection<Student>("Student");
    }

    private Student GetStudentInfoByUserName(string userName)
    {
        GetConnectionString();
        Student student = collectionUser.Find(f => f.UserName == userName).FirstOrDefault();
        return student;
    }

    [HttpGet("{userName}")]
    public IActionResult GetInformation(string userName)
    {
        return Ok(GetStudentInfoByUserName(userName));
    }

    [HttpGet("Committee/{userName}")]
    public async Task<IActionResult> GetInformationByCommittee(string userName)
    {
        GetConnectionString();
        List<Student> student = await collectionUser.Find(f => f.Thesis.CommitteeInfo.UserName == userName).ToListAsync();

        return Ok(student);
    }

    [HttpPut("{userName}")]
    public async Task<IActionResult> UpdateThesis(string userName, [FromBody] Thesis thesis)
    {
        GetConnectionString();
        Student student = collectionUser.Find(f => f.UserName == userName).FirstOrDefault();
        student.Thesis = thesis; 

        FilterDefinition<Student> filter = Builders<Student>.Filter.Where(f => f.UserName == userName);
        await collectionUser.FindOneAndReplaceAsync(filter, student);
        return Ok();
    }
}
