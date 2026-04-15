using DevUtilityHub.Api.Extensions;
using DevUtilityHub.Api.Middleware;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDevUtilityServices();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
		policy.WithOrigins(
			"https://toolkit.nikkapaola.com",
			"http://localhost:5173"
		)
		.AllowAnyHeader()
		.AllowAnyMethod());
});

builder.WebHost.ConfigureKestrel(options =>
{
	options.Limits.MaxRequestBodySize = 1_048_576; // 1 MB
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
