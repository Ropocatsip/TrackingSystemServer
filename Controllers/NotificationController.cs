using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using server.Models;
using Newtonsoft.Json;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    private readonly ILogger<NotificationController> _logger;
    IMongoCollection<Notification> collectionNoti;
    IMongoCollection<UserInfo> collectionUser;
    
    public NotificationController(ILogger<NotificationController> logger)
    {
        _logger = logger;
    }

    private void GetConnectionString()
    {
        const string connectionUri = "mongodb://localhost:27017";
        var client = new MongoClient(connectionUri);
        var database = client.GetDatabase("tracking_system");
        collectionNoti = database.GetCollection<Notification>("Notification");
        collectionUser = database.GetCollection<UserInfo>("UserInfo");

    }

    [HttpGet("{userName}")]
    public async Task<IActionResult> GetInformation(string userName)
    {
        GetConnectionString();
        List<Notification> notifications = await collectionNoti.Find(f => f.UserName == userName).SortByDescending(e => e.SendDate).ToListAsync();
        return Ok(notifications);
    }

    [HttpPost("{receiver}")]
    public async Task<IActionResult> SendNotification(string receiver, [FromBody] Notification notification)
    {
        GetConnectionString();
        notification.SendDate = DateTime.UtcNow;
        if (receiver == "everyone")
        {
            List<UserInfo> userInfos = await GetAllUserInfo();
            for (int i = 0; i < userInfos.Count; i++)
            {
                notification.UserName = userInfos[i].User.UserName;
                await collectionNoti.InsertOneAsync(notification);   
            }
        } else {
            List<UserInfo> userInfos = await GetUsersInfoByRole(receiver);
            for (int i = 0; i < userInfos.Count; i++)
            {
                notification.UserName = userInfos[i].User.UserName;
                await collectionNoti.InsertOneAsync(notification);   
            }
        }
        return Ok();
    }

    private async Task<List<UserInfo>> GetAllUserInfo()
    {
        List<UserInfo> userInfos = await collectionUser.Find(_ => true).ToListAsync();
        return userInfos;
    }

    private async Task<List<UserInfo>> GetUsersInfoByRole(string role)
    {
        List<UserInfo> userInfos = await collectionUser.Find(s => s.Role == role).ToListAsync();
        return userInfos;
    }
}