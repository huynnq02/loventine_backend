using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public string? content { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? postId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? userCommentId { get; set; }

    public string? time { get; set; }

    public string? userPostId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? parentCommentId { get; set; }

    public List<string>? childrenComments { get; set; }

    public string? replyType { get; set; }
}
