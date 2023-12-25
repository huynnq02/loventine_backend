namespace Loventine.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public async Task JoinChatRoom(string chatId, string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }
    public async Task LeaveChatRoom(string chatId, string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
    public async Task SendMessage(string chatRoomId, string userId, string message)
    {
        await Clients.Group(chatRoomId).SendAsync("receive_message", message);
    }
}
