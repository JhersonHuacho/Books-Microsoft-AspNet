using MyFirstApi.Services;
using System.Runtime.CompilerServices;

namespace MyFirstApi
{
	public static class LifetimeServicesCollectionExtensions
	{
		public static IServiceCollection AddLifetimeServices(this IServiceCollection services)
		{
			services.AddScoped<IScopedService, ScopedService>();
			services.AddTransient<ITransientService, TransientService>();
			services.AddSingleton<ISingletonService, SingletonService>();

			return services;
		}
	}
}
