using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class AdvisorController : ControllerBase
{
    private readonly ILogger<AdvisorController> _logger;
    IMongoCollection<Advisor> collectionAdvisor;
    IMongoCollection<UserInfo> collectionUserInfo;
    IMongoCollection<Student> collectionStudent;

    
    public AdvisorController(ILogger<AdvisorController> logger)
    {
        _logger = logger;
    }

    private void GetConnectionString()
    {
        const string connectionUri = "mongodb://localhost:27017";
        var client = new MongoClient(connectionUri);
        var database = client.GetDatabase("tracking_system");
        collectionAdvisor = database.GetCollection<Advisor>("Advisor");
        collectionUserInfo = database.GetCollection<UserInfo>("UserInfo");
        collectionStudent = database.GetCollection<Student>("Student");
    }

    [HttpGet("{userName}")]
    public IActionResult GetAdvisory(string userName)
    {
        GetConnectionString();
        Advisor advisor = collectionAdvisor.Find(f => f.UserName == userName).FirstOrDefault();
        return Ok(advisor);
    }

    [HttpPut("{advisorUserName}/{studentUserName}")]
    public async Task<IActionResult> UpdateAdvisoryRequest(string advisorUserName, string studentUserName,[FromBody] AdvisoryRequest advisoryRequest)
    {
        GetConnectionString();
        Advisor advisor = collectionAdvisor.Find(f => f.UserName == advisorUserName).FirstOrDefault();
        UserInfo userInfo = collectionUserInfo.Find(f => f.User.UserName == studentUserName).FirstOrDefault();
        advisoryRequest.StudentName = userInfo.Title + " " + userInfo.Name + " " + userInfo.LastName;
        advisoryRequest.SubmitDate = DateTime.UtcNow.ToString("dd/MM/yyyy");
        advisoryRequest.ThesisStatus.StatusId = 1;
        advisoryRequest.ThesisStatus.Desc = "อยู่ระหว่างพิจารณา";
        advisor.AdvisoryRequest.Add(advisoryRequest);

        FilterDefinition<Advisor> filter = Builders<Advisor>.Filter.Where(f => f.UserName == advisorUserName);
        await collectionAdvisor.FindOneAndReplaceAsync(filter, advisor);
        return Ok(advisor);
    }

    [HttpPut("{advisorUserName}")]
    public async Task<IActionResult> UpdateAdvisoryRequest(string advisorUserName,[FromBody] AdvisoryRequest advisoryRequest)
    {
        GetConnectionString();
        Advisor advisor = collectionAdvisor.Find(f => f.UserName == advisorUserName).FirstOrDefault();
        Student student = collectionStudent.Find(f => f.StudentId == advisoryRequest.StudentId).FirstOrDefault();

        advisoryRequest.UpdateDate = DateTime.UtcNow.ToString("dd/MM/yyyy");
        
        int index = advisor.AdvisoryRequest.FindIndex(s => s.StudentId == advisoryRequest.StudentId);
        if (advisoryRequest.ThesisStatus.StatusId == 0) 
        {
            advisor.AdvisoryRequest.RemoveAt(index);
            student.Thesis.Status.StatusId = 0;
            student.Thesis.Status.Desc = "ยังไม่มีอาจารย์ที่ปรึกษา";
        } 
        else {
            advisor.AdvisoryRequest[index] = advisoryRequest;
            student.Thesis.Status.StatusId = 2;
            student.Thesis.Status.Desc = "รอสอบ Proposal ครั้งที่ 1";
        }
        // update at advisor
        FilterDefinition<Advisor> filter = Builders<Advisor>.Filter.Where(f => f.UserName == advisorUserName);
        await collectionAdvisor.FindOneAndReplaceAsync(filter, advisor);

        // update at student
        
        FilterDefinition<Student> filterStudent = Builders<Student>.Filter.Where(f => f.StudentId == advisoryRequest.StudentId);
        await collectionStudent.FindOneAndReplaceAsync(filterStudent, student);
        return Ok();
    }
}