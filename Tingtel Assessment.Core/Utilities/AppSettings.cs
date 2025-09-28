using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tingtel_Assessment.Core.Utilities
{
	public class AppSettings
	{
		public string JWTSecret { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

		public string Audience { get; set; } = string.Empty;

		public string ExpiryInMinutes { get; set; } = string.Empty;
	}
}
