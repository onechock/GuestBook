using LiteDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuestBookCodeCase.Models
{
	public class PostModel
	{
		public PostModel()
		{
			Likes = new List<LikeModel>();
		}
		public PostModel(DateTime date, int userId, string publisher, string text)
		{
			Date = date;
			UserId = userId;
			Publisher = publisher;
			Text = text;
			Likes = new List<LikeModel>();
		}

		public int Id { get; set; }
		public DateTime Date { get; set; }
		public int UserId { get; set; }
		public string Publisher { get; set; }
		public string Text { get; set; }

		[BsonIgnore]
		public bool IsOwner { get; private set; }

		[BsonIgnore]
		public IEnumerable<LikeModel> Likes { get; set; }

		[BsonIgnore]
		public bool Liked { get; private set; }

		public void SetAdditionalProperties(int? userId)
		{
			if (userId != null) { 
				IsOwner = UserId == userId;
				Liked = Likes.Any(l => l.UserId == userId);
			}
		}
	}
}
