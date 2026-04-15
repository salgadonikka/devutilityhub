namespace DevUtilityHub.Api.Middleware
{
	public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
	{
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Unhandled exception on {Method} {Path}", context.Request.Method, context.Request.Path);

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = 500;

				var detail = env.IsDevelopment() ? ex.Message : null;
				var response = new { error = "An unexpected error occurred.", detail };

				await context.Response.WriteAsJsonAsync(response);
			}
		}
	}
}
