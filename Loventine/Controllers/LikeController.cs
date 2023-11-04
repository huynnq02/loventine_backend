using Loventine.Services;
using Microsoft.AspNetCore.Mvc;

namespace Loventine.Controllers
{
    [ApiController]
    [Route("api/likes")]
    public class LikeController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public LikeController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Like>>> GetLikes()
        {
            var likes = await _mongoDBService.GetLikesAsync();
            return Ok(likes);
        }

        [HttpPost]
        public async Task<ActionResult<Like>> CreateLike([FromBody] Like like)
        {
            await _mongoDBService.CreateLikeAsync(like);
            return CreatedAtAction(nameof(GetLikes), like);
        }
        [HttpDelete("{likeId}")]
        public async Task<ActionResult> Unlike(string likeId)
        {
            var success = await _mongoDBService.DeleteLikeAsync(likeId);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
