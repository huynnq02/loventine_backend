using Loventine.Models.FormData;
using Loventine.Services;
using Loventine.Validates;
using Microsoft.AspNetCore.Mvc;

namespace Loventine.Controllers
{
    [ApiController]
    [Route("api/chat_room")]
    public class ChatRoomController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public ChatRoomController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomRequest chatRoomCreateRequest)
        {
            try
            {
                // Validate the request
                var errors = ChatRoomValidate.Create(chatRoomCreateRequest.user1, chatRoomCreateRequest.user2, chatRoomCreateRequest.type.ToString());

                if (errors.Count > 0)
                {
                    Console.WriteLine("Validation errors:");
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error);
                    }
                    return BadRequest(new { success = false, errors });
                }

                // Convert request to ChatRoom object
                var chatRoom = new ChatRoom
                {
                    user1 = new UserDetail { user = chatRoomCreateRequest.user1 },
                    user2 = new UserDetail { user = chatRoomCreateRequest.user2 },
                    type = chatRoomCreateRequest.type.ToString()
                };

                // Create the chat room
                await _mongoDBService.CreateChatRoomAsync(chatRoom);

                // Return success response if needed
                return Ok(new { success = true, data = chatRoom });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error creating chat room: {ex.Message}");
                // Return an appropriate error response
                return StatusCode(500, new { success = false, errors = new List<string> { "Server Internal Error" } });
            }
        }
    }
}
