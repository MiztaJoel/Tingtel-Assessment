using Core.Interfaces.ServiceInterfaces;
using Tingtel_Assessment.Core.ExternalService;
using Tingtel_Assessment.Core.Interfaces;
using Tingtel_Assessment.Core.Repositories;
using Tingtel_Assessment.Core.Utilities;
using Tingtel_Assessment.Interfaces;
using Tingtel_Assessment.Repositories;

namespace Tingtel_Assessment.Extension
{
	public static class ServicesExtentions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			services.AddScoped<IIdentityRepository, IdentityRepository>();
			services.AddScoped<ITransactionRepository, TransactionRespository>();


			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<JwtTokenGenerator>();
			services.AddScoped<UtilityService>();

			return services;
		}
	}
}
