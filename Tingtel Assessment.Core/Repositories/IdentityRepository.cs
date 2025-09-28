
using Core.DTOs.Identity;
using Core.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Tingtel_Assessment.Core.Constant;
using Tingtel_Assessment.Core.Utilities;
using Tingtel_Assessment.DataContext;
using Tingtel_Assessment.Interfaces;
using Tingtel_Assessment.Models;
using Tingtel_Assessment.Repositories;

namespace Tingtel_Assessment.Core.Repositories
{
	public class IdentityRepository : GenericRepository<User>, IIdentityRepository
	{
		private readonly UserManager<User> _userManager;
		private readonly JwtTokenGenerator _jwtTokenGenerator;
		private readonly RoleManager<IdentityRole>  _roleManager;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;

		public IdentityRepository(ApplicationDbContext context,UserManager<User> userManager,JwtTokenGenerator jwtTokenGenerator,
			IConfiguration configuration,RoleManager<IdentityRole> roleManager, IEmailService emailService) : base(context)
		{
			this._userManager = userManager;
			this._jwtTokenGenerator = jwtTokenGenerator;
			this._configuration=configuration;
			this._roleManager = roleManager;
			this._emailService = emailService;
		}

		public async Task<(bool Success, AuthResponse Response, string ErrorMessage)> RegisterAsync(RegisterUserDto dto)
		{
			Log.Information("Attempting to register user with email {Email}", dto.Email);

			var existingUser = await _userManager.FindByEmailAsync(dto.Email);
			if (existingUser != null) 
			{
				Log.Warning("Registration failed: A user with email {Email} already exists", dto.Email);
				return (false, null!, "A user with the email already exists");
			}

			var user = new User
			{
				UserId = UtilityService.GeneratorUserId(),
				UserName = dto.UserName,
				Email = dto.Email,

			};

			var result = await _userManager.CreateAsync(user, dto.Password);
			if (!result.Succeeded) 
			{
				var errorMessage = string.Join(",", result.Errors.Select(e => e.Description));
				Log.Error("Registration failed for email {Email}:{ErrorMessage}", dto.Email, errorMessage);
			}

			Log.Information("User {Email} registered successfully with Id{UserId}", dto.Email, user.Id);

			var authResponse = new AuthResponse(
				Id: user.Id,
				Username: user.UserName,
				UserId: user.UserId,
				Email: user.Email,
				Roles: null!,
				Token: string.Empty
				);
			return (true,authResponse,null!);
		}

		public async Task<(bool Success, AuthResponse Response, string ErrorMessage)> LoginAsync(LoginDto dto)
		{
			Log.Information("Login attempt for email {Email}", dto.Email);
			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == dto.Email.ToLower());
			if(user == null)
			{
				Log.Warning("Login failed: User with email {Email} not found", dto.Email);
				return (false, null!, "Invalid credentials.");
			}
			var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

			if (!passwordValid)
			{
				Log.Warning("Login failed for user {Email}: Invalid password", dto.Email);
				return (false, null!, "Invalid credentials.");
			}

			user.LastLoginDate = DateTime.Now;
			await UpdateAsync(user);
			Log.Information("Updated last login date for user {Email}", dto.Email);

			// Generate JWT token.
			var token = await _jwtTokenGenerator.GenerateTokenAsync(user);
			Log.Information("JWT token generated for user {Email}", user.Email);

			var roles = await _userManager.GetRolesAsync(user);

			var authResponse = new AuthResponse(
				Id: user.Id,
			

				Username: user.UserName,
				UserId: user.UserId,
				Email: user.Email!,
				Roles: roles.ToArray(),
				Token: token
			);

			return (true, authResponse, null!);
		}
        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            Log.Information("Retrieving user with Id {UserId}", id);
            var user = await GetByIdAsync(id);
            if (user == null)
            {
                Log.Warning("User with Id {UserId} not found", id);
                return null!;
            }
            return MapToUserDto(user);
        }
       
		private UserDto MapToUserDto(User user)
		{
			return new UserDto
			{
				Id = user.Id,
				Username= user.UserName!,
				Email = user.Email!,
				UserId = user.UserId,
			};
		}

	}
}
