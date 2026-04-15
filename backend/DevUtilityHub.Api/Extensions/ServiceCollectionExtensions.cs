namespace DevUtilityHub.Api.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDevUtilityServices(this IServiceCollection services)
		{
			// Register services here as each feature is implemented, e.g.:
			// services.AddTransient<IEncoderService, EncoderService>();
			// services.AddTransient<IFormatterService, FormatterService>();
			// services.AddTransient<ITextService, TextService>();
			// services.AddTransient<IDiffService, DiffService>();
			// services.AddTransient<ITimeService, TimeService>();

			return services;
		}
	}
}
