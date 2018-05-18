using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
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

        public static ISiloHostBuilder AddRedisStorageProvider(this ISiloHostBuilder builder, string name, Action<RedisProviderOptions> options)
        {
            return builder.AddRedisStorageProvider(name, ob => ob.Configure(options));
        }

        public static ISiloHostBuilder AddRedisStorageProvider(this ISiloHostBuilder builder, string name, Action<OptionsBuilder<RedisProviderOptions>> configureOptions = null)
        {
            return builder.ConfigureServices(services => 
            {
                configureOptions?.Invoke(services.AddOptions<RedisProviderOptions>(name));
                services.ConfigureNamedOptionForLogging<RedisProviderOptions>(name);
                
            });
        }

        //public static void AddRedisStorageProvider(this ClusterConfiguration config, string name)
        //{
        //    RedisProviderOptions options = new RedisProviderOptions();

        //    AddRedisStorageProvider(config, name, options);
        //}

        //public static void AddRedisStorageProvider(this ClusterConfiguration config, string name, RedisProviderOptions options)
        //{
        //    config.Globals.RegisterStorageProvider<RedisProvider>(name, options.ToDictionary());
        //}
    }
}
