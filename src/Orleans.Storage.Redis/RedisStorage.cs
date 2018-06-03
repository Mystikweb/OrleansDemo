using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orleans.Runtime;
using Orleans.Serialization;
using StackExchange.Redis;

namespace Orleans.Storage.Redis
{
    public class RedisStorage : IGrainStorage, IDisposable
    {
        private readonly RedisProviderOptions options;
        private readonly ILogger logger;
        private readonly SerializationManager serializationManager;
        private readonly JsonSerializerSettings jsonSettings;
        private readonly ConfigurationOptions redisOptions;

        private ConnectionMultiplexer connectionMultiplexer;
        private IDatabase redisDatabase;

        public RedisStorage(ILoggerFactory loggerFactory, SerializationManager serializationManager, RedisProviderOptions options)
        {
            this.options = options;
            this.logger = loggerFactory.CreateLogger($"{this.GetType().FullName}");
            this.serializationManager = serializationManager;

            redisOptions = new ConfigurationOptions
            {
                ClientName = this.GetType().FullName,
                EndPoints =
                {
                    { options.Hostname, options.Port }
                },
                Password = options.Password
            };

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

        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            if (redisDatabase == null)
            {
                await ConnectToDatabase();
            }

            string key = grainReference.ToKeyString();

            logger.Debug((int)RedisProviderLogCode.ReadingRedisData, "Reading: GrainType={0} Pk={1} Grainid={2} from Database={3}",
                grainType, key, grainReference, redisDatabase.Database);

            RedisValue value = await redisDatabase.StringGetAsync(key);
            if (value.HasValue)
            {
                if (options.UseJsonFormat)
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
            if (redisDatabase == null)
            {
                await ConnectToDatabase();
            }

            var primaryKey = grainReference.ToKeyString();
            logger.Debug((int)RedisProviderLogCode.WritingRedisData, "Writing: GrainType={0} PrimaryKey={1} Grainid={2} ETag={3} to Database={4}",
                grainType, primaryKey, grainReference, grainState.ETag, redisDatabase.Database);

            var data = grainState.State;

            if (options.UseJsonFormat)
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

        public async Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            if (redisDatabase == null)
            {
                await ConnectToDatabase();
            }

            var primaryKey = grainReference.ToKeyString();
            logger.Debug((int)RedisProviderLogCode.ClearingRedisData, "Clearing: GrainType={0} Pk={1} Grainid={2} ETag={3} to Database={4}",
                grainType, primaryKey, grainReference, grainState.ETag, redisDatabase.Database);

            await redisDatabase.KeyDeleteAsync(primaryKey);
        }

        public void Dispose()
        {
            connectionMultiplexer?.Dispose();
        }

        private async Task ConnectToDatabase()
        {
            if (connectionMultiplexer == null || !connectionMultiplexer.IsConnected)
            {
                connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(redisOptions);
            }

            redisDatabase = connectionMultiplexer.GetDatabase(options.DatabaseNumber);
        }
    }

    public class RedisStorageFactory
    {
        public static IGrainStorage Create(IServiceProvider services, string name)
        {
            return ActivatorUtilities.CreateInstance<RedisStorage>(services,
                services.GetRequiredService<IOptionsSnapshot<RedisProviderOptions>>().Get(name));
        }
    }
}