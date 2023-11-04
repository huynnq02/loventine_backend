using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YourNamespace
{
    public class Post
    {
        public Post()
        {
            Comments = new List<string>();
            LikeAllUserId = new List<string>();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Author { get; set; }

        [BsonRequired]
        public string? Title { get; set; }

        [BsonRequired]
        public string? Content { get; set; }

        [BsonRequired]
        public string? PostingTime { get; set; }

        public List<string>? LikeAllUserId { get; set; }

        [BsonDefaultValue(0)]
        public int LikeCounts { get; set; }

        public List<string>? Comments { get; set; }

        [BsonDefaultValue(null)]
        public List<string>? Images { get; set; }

        public string? Emoji { get; set; }

        [BsonDefaultValue(0)]
        public int View { get; set; }

        [BsonDefaultValue(false)]
        public bool IsLike { get; set; }

        [BsonDefaultValue(false)]
        public bool IsDelete { get; set; }

        public string? DeleteTime { get; set; }

        public string? UserAddress { get; set; }

        [BsonDefaultValue(true)]
        public bool IsPublic { get; set; }

        [BsonDefaultValue(false)]
        public bool IsBookmark { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedAt { get; set; }
    }
}
