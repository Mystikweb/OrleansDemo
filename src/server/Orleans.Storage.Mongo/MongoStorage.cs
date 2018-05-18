using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Orleans.Runtime;
using Orleans.Serialization;

namespace Orleans.Storage.Mongo
{
    public class MongoStorage : IGrainStorage
    {
        private readonly MongoOptions options;
        private readonly ILogger logger;
        private readonly IMongoDatabase database;

        public MongoStorage(string name, MongoOptions options, ILoggerFactory loggerFactory)
        {
            this.options = options;
            this.logger = loggerFactory.CreateLogger($"{this.GetType().FullName}.{name}");

            MongoServerAddress address = new MongoServerAddress(this.options.Host, this.options.Port);
            MongoCredential credentials = MongoCredential.CreateCredential(this.options.DatabaseName, this.options.UserName, this.options.Password);

            MongoClientSettings connectionSettings = new MongoClientSettings
            {
                ApplicationName = name,
                Server = address,
                Credential = credentials
            };

            MongoClient client = new MongoClient(connectionSettings);
            database = client.GetDatabase(this.options.DatabaseName);
        }

        public Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            throw new System.NotImplementedException();
        }

        public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            throw new System.NotImplementedException();
        }

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            throw new System.NotImplementedException();
        }
    }
}