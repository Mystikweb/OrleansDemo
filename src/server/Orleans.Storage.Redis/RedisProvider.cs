using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Serialization;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Orleans.Storage.Redis
{
    public class RedisProvider : IStorageProvider
    {
        private ConnectionMultiplexer connectionMultiplexer;
        private IDatabase redisDatabase;
        private SerializationManager serializationManager;
        private JsonSerializerSettings jsonSettings;
        private bool useJsonFormat = true;

        public Logger Log { get; private set; }

        public string Name { get; private set; }

        public async Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;
            Log = providerRuntime.GetLogger("Orleans.Storage.Redis." + providerRuntime.ServiceId.ToString());

            serializationManager = providerRuntime.ServiceProvider.GetRequiredService<SerializationManager>();

            if (!ConfigurationExists(config, RedisProviderConstants.REDIS_CONNECTION_STRING))
            {
                throw new ApplicationException("Redis Connection String Missing!");
            }

            connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(config.Properties[RedisProviderConstants.REDIS_CONNECTION_STRING]);

            if (ConfigurationExists(config, RedisProviderConstants.REDIS_DATABASE_NUMBER))
            {
                redisDatabase = connectionMultiplexer.GetDatabase(Convert.ToInt32(config.Properties[RedisProviderConstants.REDIS_DATABASE_NUMBER]));
            }
            else
            {
                redisDatabase = connectionMultiplexer.GetDatabase();
            }

            if (ConfigurationExists(config, RedisProviderConstants.USE_JSON_FORMAT_PROPERTY))
            {
                useJsonFormat = config.Properties[RedisProviderConstants.USE_JSON_FORMAT_PROPERTY].Equals("true", StringComparison.OrdinalIgnoreCase);
            }

            jsonSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        public async Task Close()
        {
            await connectionMultiplexer.CloseAsync();
            connectionMultiplexer.Dispose();
        }

        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            string key = grainReference.ToKeyString();

            if (Log.IsVerbose3)
            {
                Log.Verbose3((int)RedisProviderLogCode.ReadingRedisData, "Reading: GrainType={0} Pk={1} Grainid={2} from Database={3}",
                    grainType, key, grainReference, redisDatabase.Database);
            }

            RedisValue value = await redisDatabase.StringGetAsync(key);
            if (value.HasValue)
            {
                if (useJsonFormat)
                {
                    grainState.State = JsonConvert.DeserializeObject(value, grainState.State.GetType(), jsonSettings);
                }
                else
                {
                    grainState.State = serializationManager.DeserializeFromByteArray<object>(value);
                }
            }

            grainState.ETag = key;
        }

        public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var primaryKey = grainReference.ToKeyString();
            if (Log.IsVerbose3)
            {
                Log.Verbose3((int)RedisProviderLogCode.WritingRedisData, "Writing: GrainType={0} PrimaryKey={1} Grainid={2} ETag={3} to Database={4}",
                    grainType, primaryKey, grainReference, grainState.ETag, redisDatabase.Database);
            }
            var data = grainState.State;

            if (useJsonFormat)
            {
                var payload = JsonConvert.SerializeObject(data, jsonSettings);
                await redisDatabase.StringSetAsync(primaryKey, payload);
            }
            else
            {
                byte[] payload = serializationManager.SerializeToByteArray(data);
                await redisDatabase.StringSetAsync(primaryKey, payload);
            }
        }

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var primaryKey = grainReference.ToKeyString();
            if (Log.IsVerbose3)
            {
                Log.Verbose3((int)RedisProviderLogCode.ClearingRedisData, "Clearing: GrainType={0} Pk={1} Grainid={2} ETag={3} to Database={4}",
                    grainType, primaryKey, grainReference, grainState.ETag, redisDatabase.Database);
            }
            
            return redisDatabase.KeyDeleteAsync(primaryKey);
        }

        private bool ConfigurationExists(IProviderConfiguration config, string key)
        {
            return config.Properties.ContainsKey(key) && !string.IsNullOrEmpty(config.Properties[key]);
        }
    }
}
