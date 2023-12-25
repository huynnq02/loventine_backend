using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonRequired]
    public string chatRoomId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonRequired]
    public string userId { get; set; }

    [BsonRequired]
    public string message { get; set; }

    [BsonRequired]
    public string type { get; set; }

    [BsonDefaultValue(false)]
    public bool? isDeleted { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement("createdAt")]
    public DateTime? createdAt { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement("updatedAt")]
    public DateTime? updatedAt { get; set; }
}
