using Loventine.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Loventine.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Bookmark> _bookmarkCollection;
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Post> _postCollection;
        private readonly IMongoCollection<Like> _likeCollection;
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMongoCollection<Message> _messageCollection;
        private readonly IMongoCollection<ChatRoom> _chatRoomCollection;
        private readonly IHubContext<ChatHub> _hubContext;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings, IHubContext<ChatHub> hubContext)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>("users");
            _postCollection = database.GetCollection<Post>("posts");
            _likeCollection = database.GetCollection<Like>("likes");
            _commentCollection = database.GetCollection<Comment>("comments");
            _messageCollection = database.GetCollection<Message>("messages");
            _chatRoomCollection = database.GetCollection<ChatRoom>("chatrooms");
            _bookmarkCollection = database.GetCollection<Bookmark>("bookmarks");

            _hubContext = hubContext;



        }
        public async Task<bool> DeleteBookmarkAsync(string bookmarkId)
        {
            try
            {
                var filter = Builders<Bookmark>.Filter.Eq("_id", bookmarkId);
                var result = await _bookmarkCollection.DeleteOneAsync(filter);

                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting bookmark: {ex.Message}");
                throw;
            }
        }
        public async Task<Bookmark> CreateBookmarkAsync(Bookmark bookmark)
        {
            try
            {
                bookmark.time = DateTime.UtcNow.ToString();

                var existingBookmarkFilter = Builders<Bookmark>.Filter.And(
                    Builders<Bookmark>.Filter.Eq("postId", bookmark.postId),
                    Builders<Bookmark>.Filter.Eq("userId", bookmark.userId)
                );

                var existingBookmark = await _bookmarkCollection.Find(existingBookmarkFilter).FirstOrDefaultAsync();

                if (existingBookmark != null)
                {
                    Console.WriteLine("Bookmark already exists for the user and post");
                    return existingBookmark;
                }

                await _bookmarkCollection.InsertOneAsync(bookmark);

                Console.WriteLine("Bookmark created successfully");
                return bookmark;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating bookmark: {ex.Message}");
                throw;
            }
        }

        public async Task<List<string>> GetBookmarksByUserIdAsync(string userId)
        {
            var user = await _userCollection.Find(u => u._id == userId).FirstOrDefaultAsync();
            if (user.bookmarks != null)
            {
                return user.bookmarks;
            }
            return new List<string>();
        }



        public async Task<Bookmark?> GetBookmarkByIdAsync(string bookmarkId)
        {
            return await _bookmarkCollection.Find(b => b._id == bookmarkId).FirstOrDefaultAsync();
        }
        //---------------------------------
        // Region of Message and ChatRoom services
        //---------------------------------
        public async Task<Message> CreateMessage(Message message)
        {
            message.createdAt = DateTime.UtcNow;
            await _messageCollection.InsertOneAsync(message);
            var filter = Builders<ChatRoom>.Filter.Eq("_id", message.chatRoomId);
            var update = Builders<ChatRoom>.Update.Set("lastMessage", message.message);

            await _chatRoomCollection.UpdateOneAsync(filter, update);
            return message;
        }
        public async Task CreateChatRoomAsync(ChatRoom chatRoom)
        {
            try
            {

                chatRoom.createdAt = DateTime.UtcNow;

                var filter = Builders<ChatRoom>.Filter.Where(c =>
                    c.user1.user == chatRoom.user1.user &&
                    c.user2.user == chatRoom.user2.user &&
                    c.type == chatRoom.type
                );

                var existingChatRoom = await _chatRoomCollection.Find(filter).FirstOrDefaultAsync();

                if (existingChatRoom != null)
                {
                    Console.WriteLine("Chat room already exists");
                    return;
                }

                await _chatRoomCollection.InsertOneAsync(chatRoom);

                Console.WriteLine("Chat room created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating chat room: {ex.Message}");
            }
        }

        // Region of User services
        public async Task<User?> LoginUserAsync(string email, string password)
        {
            var user = await _userCollection.Find(u => u.email == email).FirstOrDefaultAsync();

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.password))
            {
                return user;
            }

            return null;
        }
        public async Task CreateUserAsync(User user)
        {
            // Hash the password using BCrypt
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);

            await _userCollection.InsertOneAsync(user);
            return;
        }
        public async Task<List<User>> GetUserAsync()
        {
            return await _userCollection.Find(new BsonDocument()).ToListAsync();

        }
        public async Task<User?> GetUserByIdAsync(string userId)
        {
            var user = await _userCollection.Find(u => u._id == userId).FirstOrDefaultAsync();
            return user;
        }

        /*  public async Task AddToP1ay1istAsync(string id, string movield)
          {
              FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
              UpdateDefinition<User> update = Builders<User>.Update.AddToSet<string>("movieId", movield);
              await _userCollection.UpdateOneAsync(filter, update);
              return;
          }
          public async Task DeleteAsync(string id)
          {
              FieldDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
              await _userCollection.DeleteOneAsync(filter);
              return;
          }*/
        //---------------------------------
        // Region of Post services
        //---------------------------------

        public async Task<List<Post>> GetPostsAsync(string userId)
        {
            var user = await _userCollection.Find(u => u._id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return new List<Post>();
            }

            var posts = await _postCollection.Find(new BsonDocument()).ToListAsync();

            foreach (var post in posts)
            {
                post.isLike = post.likeAllUserId?.Contains(userId) ?? false;
            }

            return posts;
        }

        public async Task<Post?> GetPostByIdAsync(string postId)
        {
            var post = await _postCollection.Find(p => p._id == postId).FirstOrDefaultAsync();
            return post;
        }

        public async Task CreatePostAsync(Post post)
        {
            await _postCollection.InsertOneAsync(post);
        }

        public async Task<bool> UpdatePostAsync(string postId, Post updatedPost)
        {
            var result = await _postCollection.ReplaceOneAsync(p => p._id == postId, updatedPost);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeletePostAsync(string postId)
        {
            var result = await _postCollection.DeleteOneAsync(p => p._id == postId);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
        //---------------------------------
        // Region of Like services
        //---------------------------------
        public async Task<List<Like>> GetLikesAsync()
        {
            return await _likeCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task CreateLikeAsync(Like like)
        {
            var postFilter = Builders<Post>.Filter.Eq("_id", like.postId);
            var updateDefinition = Builders<Post>.Update
                .Inc("likeCounts", 1)
                .AddToSet("likeAllUserId", like.userLikeId);
            await _postCollection.UpdateOneAsync(postFilter, updateDefinition);
        }
        public async Task<bool> DeleteLikeAsync(string likeId)
        {
            var result = await _likeCollection.DeleteOneAsync(l => l._id == likeId);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
        //---------------------------------
        // Region of Comment services
        //---------------------------------
        public async Task<List<Comment>> GetCommentsForPostAsync(string postId)
        {
            var comments = await _commentCollection.Find(c => c.postId == postId).ToListAsync();
            return comments;
        }
        public async Task<List<Comment>> GetCommentsAsync()
        {

            return await _commentCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(string commentId)
        {
            var comment = await _commentCollection.Find(c => c._id == commentId).FirstOrDefaultAsync();
            return comment;
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            await _commentCollection.InsertOneAsync(comment);
            var updateDefinition = Builders<Post>.Update.Push(p => p.comments, comment._id);
            var result = await _postCollection.UpdateOneAsync(p => p._id == comment.postId, updateDefinition);
            if (comment.replyType == "comment" && !string.IsNullOrEmpty(comment.parentCommentId))
            {
                var updateParentcomment = Builders<Comment>.Update.Push(c => c.childrenComments, comment._id);
                var updateResult = await _commentCollection.UpdateOneAsync(c => c._id == comment.parentCommentId, updateParentcomment);
            }

        }

        public async Task<bool> UpdateCommentAsync(string commentId, Comment updatedComment)
        {
            var result = await _commentCollection.ReplaceOneAsync(c => c._id == commentId, updatedComment);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteCommentAsync(string commentId)
        {
            var result = await _commentCollection.DeleteOneAsync(c => c._id == commentId);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

    }
}
