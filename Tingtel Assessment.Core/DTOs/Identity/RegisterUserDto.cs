using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Identity
{
	public class RegisterUserDto
	{
		[Required(ErrorMessage = "Username is required.")]
		[StringLength(25, ErrorMessage = "Username can't be longer than 25 characters.")]
		public string UserName { get; set; }

		[Required, EmailAddress]
		[StringLength(50, ErrorMessage = "Email can't be longer than 50 characters.")]
		public string Email { get; set; }

		[Required, MinLength(6)]
		public string Password { get; set; }

	
	}
}
