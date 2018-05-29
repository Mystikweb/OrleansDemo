using System;
using MongoDB.Driver;
using Orleans.Runtime;

namespace Orleans.Storage.MongoDb.Events
{
    public class MongoDBEventOptions
    {
        public const string MONGO_EVENT_OPTIONS = "Orleans.Storage.MongoDb.Events";
        public string ConnectionString { get; set; }
        public string Database { get; set; }

        internal void Validate(string name = null)
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                string originalConnectionString = ConnectionString;

                if (string.IsNullOrWhiteSpace(Database))
                {
                    try
                    {
                        var mongoUrl = MongoUrl.Create(originalConnectionString);

                        Database = mongoUrl.DatabaseName;
                    }
                    catch
                    {
                        Database = null;
                    }
                }

                try
                {
                    MongoUrlBuilder urlBuilder = new MongoUrlBuilder(originalConnectionString) { DatabaseName = null };

                    ConnectionString = urlBuilder.ToString();
                }
                catch
                {
                    ConnectionString = originalConnectionString;
                }
            }

            string typeName = GetType().Name;

            if (!string.IsNullOrWhiteSpace(typeName))
            {
                typeName = $"{typeName} for {name}";
            }

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new OrleansConfigurationException($"Invalid {typeName} values for {nameof(ConnectionString)}. {nameof(ConnectionString)} is required.");
            }

            if (string.IsNullOrWhiteSpace(Database))
            {
                throw new OrleansConfigurationException($"Invalid {typeName} values for {nameof(Database)}. {nameof(Database)} is required.");
            }
        }
    }
}
