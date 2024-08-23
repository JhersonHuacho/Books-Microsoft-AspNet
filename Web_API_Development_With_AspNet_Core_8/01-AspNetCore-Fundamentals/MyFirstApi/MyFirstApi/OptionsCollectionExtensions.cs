using MyFirstApi.Models;

namespace MyFirstApi;

public static class OptionsCollectionExtensions
{
	public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration)
	{
		// Register the DatabaseOption class as a configuration object.
		// This line must be added before the `builder.Build()` method.
		services.Configure<DatabaseOption>(configuration.GetSection(DatabaseOption.SectionName));

		// named options
		services.Configure<DatabaseOptions>(
			DatabaseOptions.SystemDatabaseSectionName,
			configuration.GetSection($"{DatabaseOptions.SectionName}: {DatabaseOptions.SystemDatabaseSectionName}"));

		services.Configure<DatabaseOptions>(
			DatabaseOptions.BusinessDatabaseSectionName,
			configuration.GetSection($"{DatabaseOptions.SectionName}: {DatabaseOptions.BusinessDatabaseSectionName}"));

		return services;
	}
}
