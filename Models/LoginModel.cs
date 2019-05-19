using Microsoft.EntityFrameworkCore;

namespace GuestBookCodeCase.Models
{	public class LoginModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
