using Loventine.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using YourNamespace;

namespace Loventine.Services
{
    public class MongoDBService
    {

        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Post> _postCollection;
        private readonly IMongoCollection<Like> _likeCollection;
        private readonly IMongoCollection<Comment> _commentCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>("users");
            _postCollection = database.GetCollection<Post>("posts");
            _likeCollection = database.GetCollection<Like>("likes");
            _commentCollection = database.GetCollection<Comment>("comments");


        }
        // Region of User services
        public async Task<User?> LoginUserAsync(string email, string password)
        {
            var user = await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }
        public async Task CreateUserAsync(User user)
        {
            // Hash the password using BCrypt
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _userCollection.InsertOneAsync(user);
            return;
        }
        public async Task<List<User>> GetUserAsync()
        {
            return await _userCollection.Find(new BsonDocument()).ToListAsync();

        }
        public async Task<User?> GetUserByIdAsync(string userId)
        {
            var user = await _userCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
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

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _postCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(string postId)
        {
            var post = await _postCollection.Find(p => p.Id == postId).FirstOrDefaultAsync();
            return post;
        }

        public async Task CreatePostAsync(Post post)
        {
            await _postCollection.InsertOneAsync(post);
        }

        public async Task<bool> UpdatePostAsync(string postId, Post updatedPost)
        {
            var result = await _postCollection.ReplaceOneAsync(p => p.Id == postId, updatedPost);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeletePostAsync(string postId)
        {
            var result = await _postCollection.DeleteOneAsync(p => p.Id == postId);
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
            await _likeCollection.InsertOneAsync(like);
        }
        public async Task<bool> DeleteLikeAsync(string likeId)
        {
            var result = await _likeCollection.DeleteOneAsync(l => l.Id == likeId);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
        //---------------------------------
        // Region of Comment services
        //---------------------------------
        public async Task<List<Comment>> GetCommentsForPostAsync(string postId)
        {
            var comments = await _commentCollection.Find(c => c.PostId == postId).ToListAsync();
            return comments;
        }
        public async Task<List<Comment>> GetCommentsAsync()
        {

            return await _commentCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(string commentId)
        {
            var comment = await _commentCollection.Find(c => c.Id == commentId).FirstOrDefaultAsync();
            return comment;
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            await _commentCollection.InsertOneAsync(comment);
            var updateDefinition = Builders<Post>.Update.Push(p => p.Comments, comment.Id);
            var result = await _postCollection.UpdateOneAsync(p => p.Id == comment.PostId, updateDefinition);
        }

        public async Task<bool> UpdateCommentAsync(string commentId, Comment updatedComment)
        {
            var result = await _commentCollection.ReplaceOneAsync(c => c.Id == commentId, updatedComment);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteCommentAsync(string commentId)
        {
            var result = await _commentCollection.DeleteOneAsync(c => c.Id == commentId);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

    }
}
