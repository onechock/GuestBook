using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuestBookCodeCase.Models
{
	public class NewEditPostModel
	{
		public int Id { get; set; }
		public UserModel User { get; set; }
		public string Text { get; set; }
	}
}
