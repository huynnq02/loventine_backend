using Loventine.Models;
using Loventine.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly MongoDBService _mongoDBService;

    public MessagesController(IHubContext<ChatHub> hubContext, MongoDBService mongoDBService)
    {
        _hubContext = hubContext;
        _mongoDBService = mongoDBService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateMessage([FromBody] MessageRequest request)
    {
        try
        {
            var message = new Message
            {
                message = request.message,
                userId = request.userId,
                chatRoomId = request.chatRoomId,
                type = request.type,
            };

            // Save the message to MongoDB
            var savedMessage = await _mongoDBService.CreateMessage(message);

            // Send the message to connected clients using SignalR
            await _hubContext.Clients.Group(request.chatRoomId).SendAsync("receive_message", new
            {
                userId = request.userId,
                message = request.message,
                type = request.type,
                timestamp = savedMessage.createdAt
            });



            return Ok(new { Message = "Message sent successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}

public class MessageRequest
{
    public string message { get; set; }
    public string userId { get; set; }
    public string chatRoomId { get; set; }
    public string type { get; set; }
}
