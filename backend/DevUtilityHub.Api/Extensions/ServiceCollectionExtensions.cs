using DevUtilityHub.Api.Services;
using DevUtilityHub.Api.Services.Interfaces;

namespace DevUtilityHub.Api.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDevUtilityServices(this IServiceCollection services)
		{
			services.AddScoped<IFormatService, FormatService>();
			services.AddScoped<IDiffService, DiffService>();

			return services;
		}
	}
}
