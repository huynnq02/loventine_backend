using Loventine.Services;
using Microsoft.AspNetCore.Mvc;

namespace Loventine.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public CommentController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetComments()
        {
            var comments = await _mongoDBService.GetCommentsAsync();
            return Ok(comments);
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<Comment>> GetCommentById(string commentId)
        {
            var comment = await _mongoDBService.GetCommentByIdAsync(commentId);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment([FromBody] Comment comment)
        {
            await _mongoDBService.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { commentId = comment.Id }, comment);
        }

        [HttpPut("{commentId}")]
        public async Task<ActionResult> UpdateComment(string commentId, [FromBody] Comment updatedComment)
        {
            var success = await _mongoDBService.UpdateCommentAsync(commentId, updatedComment);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{commentId}")]
        public async Task<ActionResult> DeleteComment(string commentId)
        {
            var success = await _mongoDBService.DeleteCommentAsync(commentId);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsForPost(string postId)
        {
            var comments = await _mongoDBService.GetCommentsForPostAsync(postId);
            return Ok(comments);
        }
    }
}
