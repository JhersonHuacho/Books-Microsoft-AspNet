using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private readonly IPostsService _postsService;
        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet]
		public ActionResult<List<Post>> GetPosts()
		{
			return new List<Post>
			{
				new() { Id = 1, UserId = 1, Title = "Post1", Body = "Thefirst post." },
				new() { Id = 2, UserId = 1, Title = "Post2", Body = "Thesecond post." },
				new() { Id = 3, UserId = 1, Title = "Post3", Body = "Thethird post." }
			};
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Post>> GetPosts(int id)
		{
			var post = await _postsService.GetPost(id);
			if (post == null)
			{
				return NotFound();
			}
			return Ok(post);
		}

		[HttpPost]
		public async Task<ActionResult<Post>> CreatePost(Post post)
		{
			await _postsService.CreatePost(post);
			return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Post>> UpdatePost(int id, Post post)
		{
			if (id != post.Id)
			{
				return BadRequest();
			}

			var updatedPost = await _postsService.UpdatePost(id, post);
			if (updatedPost == null)
			{
				return NotFound();
			}
			return Ok(updatedPost);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeletePost(int id)
		{
			await _postsService.DeletePost(id);
			return NoContent();
		}
	}
}
