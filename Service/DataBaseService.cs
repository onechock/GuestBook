using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuestBookCodeCase.Models;

namespace GuestBookCodeCase.Service
{
	public interface IDataBaseService
	{
		IEnumerable<PostModel> GetPosts();
		PostModel GetPost(int id);
		void AddPost(PostModel newPost);
		void EditPost(PostModel post);
		void DeletePost(int id);

		PostModel ToggleLikePost(LikeModel like);

		UserModel AddUser(UserModel newUser);
		IEnumerable<UserModel> GetUsers();
		UserModel GetUser(string username, string password);
	}

	public class DataBaseService : IDataBaseService
	{
		private readonly LiteDatabase _db;

		public DataBaseService()
		{
			_db = new LiteDatabase(@"GuestBook.db");
		}


		#region PostMethods

		public IEnumerable<PostModel> GetPosts() {
			var posts = _db.GetCollection<PostModel>("posts");

			var likes = GetAllLikes();

			return posts.FindAll().Select(p =>
			{
				p.Likes = likes.Where(l => p.Id == l.PostId);
				return p;
			});
		}

		public PostModel GetPost(int postId)
		{
			var posts = _db.GetCollection<PostModel>("posts");

			var post = posts.FindOne(p => p.Id == postId);

			post.Likes = GetLikesForPost(post.Id);

			return post;
		}

		public void AddPost(PostModel newPost)
		{
			var posts = _db.GetCollection<PostModel>("posts");

			var result = posts.Insert(newPost);
		}

		public void EditPost(PostModel post)
		{
			var posts = _db.GetCollection<PostModel>("posts");

			var result = posts.Update(post);
		}

		public void DeletePost(int postId)
		{
			var posts = _db.GetCollection<PostModel>("posts");

			DeleteLikesForPost(postId);

			var result = posts.Delete(postId);
		}

		public PostModel ToggleLikePost(LikeModel like)
		{
			var post = GetPost(like.PostId);

			var likes = _db.GetCollection<LikeModel>("like");

			var currentLike = likes.FindOne(l => l.UserId == like.UserId && l.PostId == like.PostId);

			if (currentLike == null)
				likes.Insert(like);
			else
				likes.Delete(currentLike.Id);

			post.Likes = GetLikesForPost(post.Id);

			return post;
		}

		private void DeleteLikesForPost(int postId) {
			var likes = _db.GetCollection<LikeModel>("like");

			likes.Delete(l => l.PostId == postId);
		}
		private IEnumerable<LikeModel> GetAllLikes() {
			var likes = _db.GetCollection<LikeModel>("like");

			return likes.FindAll();
		}
		private IEnumerable<LikeModel> GetLikesForPost(int postId) {
			var likes = _db.GetCollection<LikeModel>("like");

			return likes.Find(p => p.PostId == postId);
		}

		#endregion PostMethods


		#region UserMethods
		public UserModel AddUser(UserModel newUser)
		{
			var users = _db.GetCollection<UserModel>("users");

			var user = users.FindOne(u => u.Username == newUser.Username);

			if (user != null) {
				throw new Exception("User allready exist");
			}

			var result = users.Insert(newUser);

			return newUser;
		}
		
		public IEnumerable<UserModel> GetUsers()
		{
			var usersCollection = _db.GetCollection<UserModel>("users");

			return usersCollection.FindAll();
		}

		public UserModel GetUser(string username, string password)
		{
			var users = _db.GetCollection<UserModel>("users");

			var user = users.FindOne(u => u.Username == username && u.Password == password);

			return user;
		}

		#endregion UserMethods

	}
}
