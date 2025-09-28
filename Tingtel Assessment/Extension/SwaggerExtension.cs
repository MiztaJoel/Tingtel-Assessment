
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Tingtel_Assessment.Extension
{
	public static class SwaggerExtension
	{
	
		public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Hotel Management System",
					Version = "v1",
					Description = "An API for Hotel Management System",

				});
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					In = ParameterLocation.Header,
					Description = "Fill in the Jwt",
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id="Bearer",
							}
						},
						new List<string>()
					}

				});
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
				c.EnableAnnotations();
			});
		

			return services;
		}

		public static WebApplication ConfigureSwaggerExplorer(this WebApplication app)
		{
			// Configure the HTTP request pipeline.
			app.UseSwagger();
			app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Management System API V1");
			});

			return app;
		}
	}
}
