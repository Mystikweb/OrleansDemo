using Orleans.Runtime.Configuration;
using System.Collections.Generic;

namespace Orleans.Storage.Redis
{
    public static class RedisProviderExtensions
    {
        public static IDictionary<string, string> ToDictionary(this RedisProviderOptions options)
        {
            return new Dictionary<string, string>
            {
                { RedisProviderConstants.REDIS_CONNECTION_STRING, options.RedisConnectionString },
                { RedisProviderConstants.REDIS_DATABASE_NUMBER, options.DatabaseNumber.ToString() },
                { RedisProviderConstants.USE_JSON_FORMAT_PROPERTY, options.UseJsonFormat.ToString() }
            };
        }

        public static void AddRedisStorageProvider(this ClusterConfiguration config, string name)
        {
            RedisProviderOptions options = new RedisProviderOptions();

            AddRedisStorageProvider(config, name, options);
        }

        public static void AddRedisStorageProvider(this ClusterConfiguration config, string name, RedisProviderOptions options)
        {
            config.Globals.RegisterStorageProvider<RedisProvider>(name, options.ToDictionary());
        }
    }
}
