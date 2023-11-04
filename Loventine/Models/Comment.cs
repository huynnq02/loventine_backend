using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Loventine
{
    public class Comment
    {
        public Comment()
        {
            ChildrenComments = new List<string>();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired]
        public string? Content { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? PostId { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserCommentId { get; set; }

        public string? UserPostId { get; set; }

        public string? ParentCommentId { get; set; }
        public List<string>? ChildrenComments { get; set; }

        public string? ReplyType { get; set; }
    }
}
