
using Core.DTOs.Identity;
using Tingtel_Assessment.Core.Constant;

namespace Tingtel_Assessment.Interfaces
{
	public interface IIdentityRepository
	{
		Task<(bool Success, AuthResponse Response, string ErrorMessage)> RegisterAsync(RegisterUserDto dto);

		Task<(bool Success, AuthResponse Response, string ErrorMessage)> LoginAsync(LoginDto dto);

        Task<UserDto> GetUserByIdAsync(string id);
	}
}
