using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GuestBookCodeCase.Service;
using GuestBookCodeCase.Models;
using Microsoft.AspNetCore.Authorization;

namespace GuestBookCodeCase.Controllers
{
	[Route("api/[controller]")]
	public partial class GuestBookController : Controller
	{
		private readonly IDataBaseService dataBaseService;

		public GuestBookController(IDataBaseService dataBaseService)
		{
			this.dataBaseService = dataBaseService;
		}

		[HttpGet("[action]/{id:int?}"), Authorize]
		public IEnumerable<PostModel> Posts(int? id)
		{
			var posts = dataBaseService.GetPosts();

			return posts.Select(s =>
			{
				if (id != null)
					s.SetAdditionalProperties(id);
				return s;
			}).OrderByDescending(d => d.Date);
		}

		[HttpPut("[action]"), Authorize]
		public IActionResult NewPost([FromBody]NewEditPostModel newPostModel) {
			var user = newPostModel.User;

			var post = new PostModel(
				DateTime.Now,
				user.Id,
				$"{user.FirstName} {user.LastName}",
				newPostModel.Text);

			dataBaseService.AddPost(post);

			post.SetAdditionalProperties(newPostModel.User.Id);

			return Ok(new { post });
		}

		[HttpPut("[action]"), Authorize]
		public IActionResult EditPost([FromBody]NewEditPostModel editPostModel)
		{

			var post = dataBaseService.GetPost(editPostModel.Id);

			if (post == null)
				return NotFound();

			post.Text = editPostModel.Text;

			dataBaseService.EditPost(post);

			post.SetAdditionalProperties(editPostModel.User.Id);

			return Ok(new { post });
		}

		[HttpPut("[action]/{id:int}"), Authorize]
		public IActionResult DeletePost(int id)
		{
			dataBaseService.DeletePost(id);

			return Ok(new { });
		}

		[HttpPut("[action]"), Authorize]
		public IActionResult LikePost([FromBody]LikeModel likeModel) {

			var post = dataBaseService.ToggleLikePost(likeModel);

			post.SetAdditionalProperties(likeModel.UserId);

			return Ok(new { post });
		}
	}
}
