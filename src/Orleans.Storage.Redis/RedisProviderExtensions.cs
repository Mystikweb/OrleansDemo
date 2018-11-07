using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using System;

namespace Orleans.Storage.Redis
{
    public static class RedisProviderExtensions
    {
        public static ISiloHostBuilder AddRedisGrainStorage(this ISiloHostBuilder builder, string name,
            Action<OptionsBuilder<RedisProviderOptions>> configureOptions = null)
        {
            return builder.ConfigureServices(services => services.AddRedisGrainStorage(name, configureOptions));
        }

        public static IServiceCollection AddRedisGrainStorage(this IServiceCollection services, string name,
            Action<OptionsBuilder<RedisProviderOptions>> configureOptions = null)
        {
            configureOptions?.Invoke(services.AddOptions<RedisProviderOptions>(name));

            services.ConfigureNamedOptionForLogging<RedisProviderOptions>(name);
            services.AddSingletonNamedService(name, RedisStorageFactory.Create);

            return services;
        }
    }
}
