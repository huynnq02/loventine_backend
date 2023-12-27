using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? author { get; set; }

    public string? title { get; set; }
    public string? content { get; set; }
    public string? postingTime { get; set; } = DateTime.Now.ToString();
    public List<string>? likeAllUserId { get; set; } = new List<string>();
    public int? likeCounts { get; set; } = 0;
    public List<string>? comments { get; set; } = new List<string>();
    public List<string>? images { get; set; } = new List<string>();
    public string? postType { get; set; }
    public decimal? price { get; set; }
    public string? adviseType { get; set; }

    public string? emoji { get; set; }
    public int? view { get; set; } = 0;
    public int? applyCount { get; set; } = 0;
    public bool? isLike { get; set; } = false;
    public bool? isDelete { get; set; } = false;
    public string? deleteTime { get; set; }
    public string? userAddress { get; set; }
    public bool? isPublic { get; set; } = true;
    public bool? isBookmark { get; set; } = false;
    public int? countPaymentVerified { get; set; } = 0;
    public int? numConsultingJobStart { get; set; } = 0;
    public int? countPayment { get; set; } = 0;

}

public static class ADVICE_TYPE
{
    public const string HOURLY = "Hourly";
    public const string DAILY = "Daily";
    public const string MONTHLY = "Monthly";
    public const string YEARLY = "Yearly";
}

public static class POST_TYPE
{
    public const string TYPE1 = "Type1";
    public const string TYPE2 = "Type2";
}
