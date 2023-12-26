using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public List<string>? reviews { get; set; } = new List<string>();
    public List<string>? bookmarks { get; set; } = new List<string>();
    public string? phone { get; set; }

    [BsonRequired]
    public string? password { get; set; }

    [BsonRequired]
    public string? passwordConfirm { get; set; }
    public string? token { get; set; }

    public string? email { get; set; }
    public string? birthday { get; set; }

    public bool? verified { get; set; } = false;
    public string? name { get; set; }

    public string? sex { get; set; } = "Không muốn trả lời";
    public string? avatarUrl { get; set; }
    public string? about { get; set; }
    public string? bio { get; set; }
    public string? address { get; set; }

    public List<string>? skills { get; set; } = new List<string>();
    public List<string>? jobs { get; set; } = new List<string>();
    public List<string>? works { get; set; } = new List<string>();
    public List<string>? resumes { get; set; } = new List<string>();
    public List<string>? educations { get; set; } = new List<string>();
    public List<string>? languages { get; set; } = new List<string>();
    public List<string>? image_uploads { get; set; } = new List<string>();

    public bool? online { get; set; } = false;
    public string? time { get; set; } = DateTime.UtcNow.ToString();

    public bool? isCalling { get; set; } = false;
    public bool? isReadyForMeeting { get; set; } = false;
    public string? avatar_cloudinary_public_id { get; set; }

    public bool? isVerified { get; set; } = false;
    public bool? isLoggingIn { get; set; } = false;
    public List<string>? fcm_tokens { get; set; } = new List<string>();
    public int? num_message_unwatch { get; set; } = 0;
    public int? num_notification_unwatch { get; set; } = 0;
    public decimal? wallet { get; set; } = 0;
    public decimal? walletForVerify { get; set; } = 0;
    public string? walletPassword { get; set; }
    public int? loginAttempts { get; set; } = 0;
}

