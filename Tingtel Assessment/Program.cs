 using Tingtel_Assessment.Middlewares;
using Tingtel_Assessment.Core.Utilities;
using Tingtel_Assessment.Extension;
using Tingtel_Assessment.DataContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerExplorer()
                 .InjectDbContext(builder.Configuration)
                 .AddAppConfig(builder.Configuration)
                 .AddIdentityHandlerAndStores()
                 .ConfigureIdentityOptions()
                 .AddIdentityAuth(builder.Configuration)
                 .AddApplicationServices(builder.Configuration);

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

  
var app = builder.Build();
using (var scope = app.Services.CreateScope())
		 {
			 var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			 dbContext.Database.Migrate();
		 }

// Configure the HTTP request pipeline.
app.ConfigureSwaggerExplorer()
    .ConfigureCORS(builder.Configuration)
    .AddIdentityAuthMiddlewares()
    .UseMiddleware<ExceptionHandling>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
