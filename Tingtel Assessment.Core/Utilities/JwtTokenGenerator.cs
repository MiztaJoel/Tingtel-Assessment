
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tingtel_Assessment.Models;

namespace Tingtel_Assessment.Core.Utilities
{
	public class JwtTokenGenerator
	{
		private readonly UserManager<User> _userManager;
		private readonly AppSettings _settings;

		public JwtTokenGenerator(UserManager<User> userManager,IOptions<AppSettings> options)
		{
		
			_userManager = userManager;
			_settings = options.Value;
			
		}

		public async Task<string> GenerateTokenAsync(User user)
		{
			var secretKey = _settings.JWTSecret;
			var issuer = _settings.Issuer;
			var audience = _settings.Audience;
			var expirationMinute = int.Parse(_settings.ExpiryInMinutes ?? "120");

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
			var expiration = DateTime.UtcNow.AddMinutes(expirationMinute);

			var userClaims = await _userManager.GetClaimsAsync(user);
			var role = await _userManager.GetRolesAsync(user);

			var roleClaims = role.Select(role => new Claim(ClaimTypes.Role, role));

			ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email!),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim("UserID",user.Id.ToString()),
			}
			.Union(userClaims)
			.Union(roleClaims));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = claims,
				Expires = expiration,
				Issuer=issuer,
				SigningCredentials =credentials
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);

		}

	}
}
