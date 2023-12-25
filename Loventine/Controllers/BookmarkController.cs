using Loventine.Services;
using Microsoft.AspNetCore.Mvc;

namespace Loventine.Controllers
{
    [ApiController]
    [Route("api/bookmarks")]
    public class BookmarkController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public BookmarkController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<Bookmark>>> GetBookmarks(string userId)
        {
            try
            {
                var bookmarks = await _mongoDBService.GetBookmarksByUserIdAsync(userId);
                var bookmarkList = new List<Bookmark>();

                foreach (var bookmarkId in bookmarks)
                {
                    var bookmark = await _mongoDBService.GetBookmarkByIdAsync(bookmarkId);

                    if (bookmark != null)
                    {
                        bookmarkList.Add(bookmark);
                    }
                }

                return Ok(bookmarkList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting bookmarks: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{bookmarkId}")]
        public async Task<ActionResult<Bookmark>> GetBookmarkById(string bookmarkId)
        {
            try
            {
                var bookmark = await _mongoDBService.GetBookmarkByIdAsync(bookmarkId);

                if (bookmark == null)
                {
                    return NotFound();
                }

                return Ok(bookmark);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting bookmark by ID: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Bookmark>> CreateBookmark([FromBody] Bookmark bookmark)
        {
            try
            {
                var createdBookmark = await _mongoDBService.CreateBookmarkAsync(bookmark);
                return CreatedAtAction(nameof(GetBookmarkById), new { bookmarkId = createdBookmark._id }, createdBookmark);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating bookmark: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{bookmarkId}")]
        public async Task<ActionResult> DeleteBookmark(string bookmarkId)
        {
            try
            {
                var success = await _mongoDBService.DeleteBookmarkAsync(bookmarkId);

                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting bookmark: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
