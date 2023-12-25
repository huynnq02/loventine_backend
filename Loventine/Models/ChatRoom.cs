using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

public class ChatRoom
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public UserDetail user1 { get; set; }

    public UserDetail user2 { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? lastMessage { get; set; }

    public string type { get; set; } = ChatRoomType.Private.ToString();

    public bool isExprired { get; set; } = false;

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? createdAt { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? updatedAt { get; set; }
}


public class UserDetail
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string user { get; set; }
    public int num_unwatched { get; set; } = 0;
    public bool isAllowNotifiCation { get; set; } = true;
}

public class UserData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string user { get; set; }
}
public enum ChatRoomType
{
    [EnumMember(Value = "Private")]
    Private,

    [EnumMember(Value = "Matching")]
    Matching
}