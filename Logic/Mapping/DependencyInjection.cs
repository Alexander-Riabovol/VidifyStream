using Data.Context;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Logic.Mapping
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Add Mapster mappings. Must be called after all needed services have been added into DI.
        /// </summary>
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            //Scan Data and Logic assembly for the types
            config.Scan(Assembly.GetAssembly(typeof(DataContext))!, Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            // The Work around to use DI in IRegister classes:
            // Inject IRegister mapping configurations as Scoped services via DI
            services.AddScoped<NotificationMappingConfig>();
            // Create a temporary ServiceProvider
            var provider = services.BuildServiceProvider();
            // Apply only IRegister classes that use DI
            config.Apply(
                provider.GetService<NotificationMappingConfig>()!
                );

            return services;
        }
    }
}
