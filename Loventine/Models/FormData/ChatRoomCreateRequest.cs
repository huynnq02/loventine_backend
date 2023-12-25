namespace Loventine.Models.FormData
{
    public class CreateChatRoomRequest
    {
        public string user1 { get; set; }
        public string user2 { get; set; }
        public ChatRoomType type { get; set; }
    }
    public enum ChatRoomType
    {
        Private,
        Matching,
    }
}
