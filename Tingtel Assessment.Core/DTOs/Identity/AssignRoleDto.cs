using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Identity
{
	public class AssignRoleDto
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		public string Role { get; set; }
	}
}
