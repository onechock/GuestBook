using GuestBookCodeCase.Helpers;
using GuestBookCodeCase.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GuestBookCodeCase.Service
{
	public interface IUserService
	{
		UserModel Authenticate(string username, string password);
		IEnumerable<UserModel> GetAll();

		UserModel CreateUser(UserModel user);
	}

	public class UserService : IUserService
	{
		private readonly IDataBaseService _dataBaseService;
		private readonly AppSettings _appSettings;

		public UserService(IOptions<AppSettings> appSettings, IDataBaseService dataBaseService)
		{
			_appSettings = appSettings.Value;
			_dataBaseService = dataBaseService;
		}

		public UserModel Authenticate(string username, string password)
		{
			var user = _dataBaseService.GetUser(username, password);

			// return null if user not found
			if (user == null)
				return null;

			// authentication successful so generate jwt token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			user.Token = tokenHandler.WriteToken(token);

			// remove password before returning
			user.Password = null;

			return user;
		}

		public UserModel CreateUser(UserModel newUser)
		{
			return _dataBaseService.AddUser(newUser);
		}

		public IEnumerable<UserModel> GetAll()
		{
			// return users without passwords
			return _dataBaseService.GetUsers().Select(x => {
				x.Password = null;
				return x;
			});
		}
	}
}
