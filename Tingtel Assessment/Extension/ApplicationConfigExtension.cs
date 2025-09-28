
using Tingtel_Assessment.Core.Utilities;

namespace Tingtel_Assessment.Extension
{
	public static class ApplicationConfigExtension
	{


		public static IServiceCollection AddCORS(this IServiceCollection services)
		{
			services.AddCors(options => 
			{

				options.AddPolicy("Corspolicy", policy =>
					{
						policy.AllowAnyOrigin()
							.AllowAnyMethod()
							.AllowAnyHeader();
					});
		});
			return services;
		}

	

		public static WebApplication ConfigureCORS(this WebApplication app,IConfiguration config)
		{
			app.UseCors();
				return app;
		}

		public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<AppSettings>(config.GetSection("AppSettings"));
			return services;
		}
	}
}
