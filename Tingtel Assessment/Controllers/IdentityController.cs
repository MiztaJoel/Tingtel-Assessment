using Core.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tingtel_Assessment.Interfaces;
using Tingtel_Assessment.Models;

namespace Tingtel_Assessment.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IdentityController : ControllerBase
	{
		private readonly IIdentityRepository _identityRepo;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<User> _userManager;
	
		public IdentityController(IIdentityRepository identityRepository,IHttpContextAccessor contextAccessor,UserManager<User> userManager )
		{
			_identityRepo = identityRepository;
			_contextAccessor = contextAccessor;
			_userManager = userManager;
		}

		///<summary>
		///Register a new user
		///</summary>
		///<param name="registerDto"></param>			
		///<returns></returns>
		[HttpPost("register")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _identityRepo.RegisterAsync(registerDto);
			if (!result.Success)
			{
				return BadRequest(new { message = result.ErrorMessage });
			}

			return CreatedAtAction(nameof(GetUserById), new { id = result.Response.Id }, result.Response);
		}
		///<summary>
		///Logs in an existing user and returns a JWT token
		/// </summary>
		///<param name="loginDto"></param>
		///<returns></returns>
		[HttpPost("login")]
		[AllowAnonymous]
		[SwaggerOperation(Summary ="Login use",Description ="To Login users")]
		[ProducesResponseType(typeof(AuthResponse),StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var (Success, Token, ErrorMessage) = await _identityRepo.LoginAsync(loginDto);
			if (!Success)
			{
				return Unauthorized(new { message = ErrorMessage });
			}

			return Ok(new { token = Token });
		}



		/// <summary>
		/// Retrieves a user by Id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[Authorize]
		[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetUserById(string id)
		{
			var user = await _identityRepo.GetUserByIdAsync(id);
			if (user == null) return NotFound(new { message = "User not found." });

			return Ok(user);
		}

	}
}
