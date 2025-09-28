using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tingtel_Assessment.Models
{
	public class User:IdentityUser
	{
		[PersonalData]
		public string UserId { get; set; }

		public DateTime? LastLoginDate { get; set; }

		public DateTime? SubmittedOn { get; set; }

		public string? ModifiedBy { get; set; }

		public string? ModifiedOn { get; set; }	
	}
}
