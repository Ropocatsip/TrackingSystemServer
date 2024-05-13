using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    private readonly ILogger<CourseController> _logger;
    IMongoCollection<Course> collectionUser;
    
    public CourseController(ILogger<CourseController> logger)
    {
        _logger = logger;
    }

    private void GetConnectionString()
    {
        const string connectionUri = "mongodb://localhost:27017";
        var client = new MongoClient(connectionUri);
        var database = client.GetDatabase("tracking_system");
        collectionUser = database.GetCollection<Course>("Course");
    }

    [HttpGet("")]
    public async Task<IActionResult> GetCourses()
    {
        GetConnectionString();
        List<Course> courses = await collectionUser.Find(_ => true).ToListAsync();
        return Ok(courses);
    }

    [HttpGet("{courseId}")]
    public async Task<IActionResult> GetCourseById(string courseId)
    {
        GetConnectionString();
        Course course = await collectionUser.Find(f => f.CourseId == courseId).FirstOrDefaultAsync();
        return Ok(course);
    }

    [HttpPut("{courseId}/{subjectId}")]
    public async Task<IActionResult> DeleteSubjectByCourseId(string courseId, int subjectId)
    {
        GetConnectionString();
        Course course = await collectionUser.Find(f => f.CourseId == courseId).FirstOrDefaultAsync();

        List<Subject> newSubject = new List<Subject>();
        for (int i = 0; i < course.Subject.Count; i++)
        {
            if (course.Subject[i].SubjectId != subjectId) 
            {
                newSubject.Add(course.Subject[i]);
            }
        }

        course.Subject = newSubject;
        FilterDefinition<Course> filter = Builders<Course>.Filter.Where(f => f.CourseId == courseId);
        await collectionUser.FindOneAndReplaceAsync(filter, course);

        return Ok();
    }

    [HttpPut("{courseId}")]
    public async Task<IActionResult> UpdateSubjectByCourseId(string courseId, [FromBody] Subject subject)
    {
        GetConnectionString();
        Course course = await collectionUser.Find(f => f.CourseId == courseId).FirstOrDefaultAsync();

        course.Subject.Add(subject);

        FilterDefinition<Course> filter = Builders<Course>.Filter.Where(f => f.CourseId == courseId);
        await collectionUser.FindOneAndReplaceAsync(filter, course);

        return Ok();
    }
}