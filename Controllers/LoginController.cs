using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using server.Models;
using Newtonsoft.Json;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    IMongoCollection<UserInfo> collectionUser;
    
    public LoginController(ILogger<LoginController> logger)
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
}
