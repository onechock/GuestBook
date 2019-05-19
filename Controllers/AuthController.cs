using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GuestBookCodeCase.Models;
using GuestBookCodeCase.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GuestBookCodeCase.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;

		public AuthController(IUserService userService)
		{
			_userService = userService;
		}

		// GET api/values
		[HttpPost, Route("login")]
		public IActionResult Login([FromBody]LoginModel loginModel)
		{
			if (loginModel == null)
			{
				return BadRequest("Invalid client request");
			}


			var user = _userService.Authenticate(loginModel.UserName, loginModel.Password);
			if (user != null)
			{
				return Ok(new { user });
			}
			else
			{
				return Unauthorized();
			}
		}

		

		[HttpPost, Route("CreateUser")]
		public IActionResult CreateUser([FromBody]UserModel userModel) {
			var user = _userService.CreateUser(userModel);

			return Ok(new { user });
		}


		
	}
}