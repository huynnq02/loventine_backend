/*using MongoDB.Driver;

public class ChatRepository
{
    private readonly IMongoCollection<Message> _messageCollection;
    private readonly IMongoCollection<ChatRoom> _chatRoomCollection;

    public ChatRepository(IMongoDatabase database)
    {
        _messageCollection = database.GetCollection<Message>("messages");
        _conversationCollection = database.GetCollection<ChatRoom>("conversations");
    }

    // Methods for storing and retrieving messages and conversations
    // e.g., SendMessage, GetMessages, CreateConversation, GetConversationById, etc.
}
*/