using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;

namespace Orleans.Storage.Redis
{
    public static class RedisProviderExtensions
    {
        public static IDictionary<string, string> ToDictionary(this RedisProviderOptions options)
        {
            return new Dictionary<string, string>
            {
                { RedisProviderConstants.REDIS_HOSTNAME, options.Hostname },
                { RedisProviderConstants.REDIS_PORT, options.Port.ToString() },
                { RedisProviderConstants.REDIS_PASSWORD, options.Password },
                { RedisProviderConstants.REDIS_DATABASE_NUMBER, options.DatabaseNumber.ToString() },
                { RedisProviderConstants.USE_JSON_FORMAT_PROPERTY, options.UseJsonFormat.ToString() }
            };
        }

        public static ISiloHostBuilder AddRedisGrainStorage(this ISiloHostBuilder builder, string name,
            Action<RedisProviderOptions> configureOptions)
        {
            return builder.AddRedisGrainStorage(name, ob => ob.Configure(configureOptions));
        }

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
