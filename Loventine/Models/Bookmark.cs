using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Bookmark
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? postId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string? userId { get; set; }

    public string? time { get; set; }
}
