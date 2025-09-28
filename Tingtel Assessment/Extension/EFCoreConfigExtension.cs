
using Microsoft.EntityFrameworkCore;
using Tingtel_Assessment.DataContext;

namespace Tingtel_Assessment.Extension
{
	public static class EFCoreConfigExtension
	{
		public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.GetConnectionString("apicon")));
			return services;
		}
		
	}
}
