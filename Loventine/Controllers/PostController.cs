using Loventine.Services;
using Microsoft.AspNetCore.Mvc;
using YourNamespace;

namespace Loventine.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public PostController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts()
        {
            var posts = await _mongoDBService.GetPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<Post>> GetPostById(string postId)
        {
            var post = await _mongoDBService.GetPostByIdAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
        {
            await _mongoDBService.CreatePostAsync(post);
            return CreatedAtAction(nameof(GetPostById), new { postId = post.Id }, post);
        }

        [HttpPut("{postId}")]
        public async Task<ActionResult> UpdatePost(string postId, [FromBody] Post updatedPost)
        {
            var success = await _mongoDBService.UpdatePostAsync(postId, updatedPost);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult> DeletePost(string postId)
        {
            var success = await _mongoDBService.DeletePostAsync(postId);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
