using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Loventine.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? Reviews { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? Bookmarks { get; set; }

        public string? Phone { get; set; }

        [BsonRequired]
        public string Password { get; set; }

        public string? Token { get; set; }

        public string? Email { get; set; }

        public string? Birthday { get; set; }

        public bool Verified { get; set; }

        [BsonRequired]
        public string Name { get; set; }

        public string? Sex { get; set; }

        public string? AvatarUrl { get; set; }

        public string? About { get; set; }

        public string? Bio { get; set; }

        public string? Address { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? Skills { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? Jobs { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? Works { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? Resumes { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? Educations { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? Languages { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? ImageUploads { get; set; }

        public bool? Online { get; set; }

        public string? Time { get; set; }

        public bool? IsCalling { get; set; }

        public bool? IsReadyForMeeting { get; set; }

        public string? AvatarCloudinaryPublicId { get; set; }

        public bool? IsVerified { get; set; }

        public bool? IsLoggingIn { get; set; }

        [BsonIgnoreIfDefault]
        public List<string>? FcmTokens { get; set; }

        public int? NumMessageUnwatch { get; set; }

        public int? NumNotificationUnwatch { get; set; }
    }
}
