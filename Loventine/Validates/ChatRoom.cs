using MongoDB.Bson;

namespace Loventine.Validates
{
    public class ChatRoomValidate
    {
        public static List<string> Create(string user1, string user2, string type)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(user1))
            {
                errors.Add("user1 is required");
            }
            else
            {
                if (!ObjectId.TryParse(user1, out _))
                {
                    errors.Add("user1 is not a valid object id");
                }
            }

            if (string.IsNullOrEmpty(user2))
            {
                errors.Add("user2 is required");
            }
            else
            {
                if (!ObjectId.TryParse(user2, out _))
                {
                    errors.Add("user2 is not a valid object id");
                }
            }

            if (!string.IsNullOrEmpty(user1) && !string.IsNullOrEmpty(user2))
            {
                if (user1 == user2)
                {
                    errors.Add("user1, user2 must be different");
                }
            }

            if (string.IsNullOrEmpty(type))
            {
                errors.Add("type is required");
            }
            else
            {
                if (!Enum.GetNames(typeof(ChatRoomType)).Contains(type))
                {
                    errors.Add("type is wrong, try these values: " + string.Join(", ", Enum.GetNames(typeof(ChatRoomType))));
                }
            }

            return errors;
        }

    }
}
