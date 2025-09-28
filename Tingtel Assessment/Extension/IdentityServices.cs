using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tingtel_Assessment.DataContext;
using Tingtel_Assessment.Models;

namespace Tingtel_Assessment.Extension
{
	public static  class IdentityServices
	{
		public static IServiceCollection AddIdentityHandlerAndStores(this IServiceCollection services) 
		{

			services.AddIdentityApiEndpoints<User>()
					.AddRoles<IdentityRole>()
					.AddEntityFrameworkStores<ApplicationDbContext>();
			return services;
		}

		public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
		{
			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.User.RequireUniqueEmail = true;

			});

			return services;
		}

		public static IServiceCollection AddIdentityAuth(this IServiceCollection services,IConfiguration config)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(y =>
			{
				y.SaveToken = false;
				y.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(config["AppSettings:JWTSecret"]!)),
					ValidateIssuer=false,
					ValidateAudience=false,
				};
			});

			//Add Authorization in Global Level And Policy 


			services.AddAuthorization(options =>
			{
				options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
										.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
										.RequireAuthenticatedUser()
										.Build();
			});
			return services;
		}

		public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
		{

			app.UseAuthentication();
			app.UseAuthorization();
			return app;

		}
	}
}
