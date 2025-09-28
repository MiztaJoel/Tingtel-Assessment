
using System.Net;
using System.Text.Json;
using Tingtel_Assessment.Core.Utilities;

namespace Tingtel_Assessment.Middlewares
{
	public class ExceptionHandling
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandling> _logger;

		public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger) => (_next, _logger) = (next, logger);

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex,ex.Message);
				await HandleExceptionAsync(httpContext,ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";

			if (exception is NotFoundException)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}
			else
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}

			var response = new
			{
				StatusCode = context.Response.StatusCode,
				Message = "An unexpected error occurred.",
				Details = exception.Message
			};

			return context.Response.WriteAsync(JsonSerializer.Serialize(response));
		}
	}
}
