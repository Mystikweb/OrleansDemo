using Microsoft.Extensions.DependencyInjection;
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

        public static ISiloHostBuilder ConfigureRediStorageProvider(this ISiloHostBuilder builder, string name, Action<RedisProviderOptions> options = null)
        {
            builder.ConfigureServices(services =>
            {
                if (options != null)
                {
                    services.Configure(options);
                }
                else
                {
                    services.Configure<RedisProviderOptions>(config => new RedisProviderOptions());
                }
            });

            

            return builder;
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
