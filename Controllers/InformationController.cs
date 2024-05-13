using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using server.Models;
using Newtonsoft.Json;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class InformationController : ControllerBase
{
    private readonly ILogger<InformationController> _logger;
    IMongoCollection<UserInfo> collectionUser;
    
    public InformationController(ILogger<InformationController> logger)
    {
        _logger = logger;
    }

    private void GetConnectionString()
    {
        const string connectionUri = "mongodb://localhost:27017";
        var client = new MongoClient(connectionUri);
        var database = client.GetDatabase("tracking_system");
        collectionUser = database.GetCollection<UserInfo>("UserInfo");
    }

    private UserInfo GetInfoByUserName(string userName)
    {
        GetConnectionString();
        UserInfo userInfo = collectionUser.Find(f => f.User.UserName == userName).FirstOrDefault();
        return userInfo;
    }

    [HttpGet("{userName}")]
    public IActionResult GetInformation(string userName)
    {
        return Ok(GetInfoByUserName(userName));
    }

    [HttpGet("{userName}/{password}")]
    public IActionResult GetInformation(string userName, string password)
    {
        GetConnectionString();
        UserInfo userInfo = collectionUser.Find(f => f.User.UserName == userName).FirstOrDefault();
        if (userInfo == null) return BadRequest("User Name ไม่ถูกต้อง");
        if (userInfo.User.Password != password) return BadRequest("Password ไม่ถูกต้อง");
        return Ok(userInfo);
    }

    [HttpGet("users/{role}")]
    public async Task<IActionResult> GetAllUserByRole(string role)
    {
        GetConnectionString();
        List<UserInfo> userInfos = await collectionUser.Find(f => f.Role == role).ToListAsync();
        return Ok(userInfos);
    }
}
