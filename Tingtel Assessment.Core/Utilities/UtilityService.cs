using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tingtel_Assessment.Core.Utilities
{
	public class UtilityService
	{
		public static string GeneratorUserId()
		{
			return $"US-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
		}

        public static string GenerateCategoryId()
        {
            return $"CAT-{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}";
        }


        public static async Task<string> LoadTemplateAsync(string templateFileName)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", templateFileName);
			return await File.ReadAllTextAsync(filePath);
		}
	}
}
