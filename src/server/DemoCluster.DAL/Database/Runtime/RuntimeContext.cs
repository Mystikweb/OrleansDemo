
using MongoDB.Driver;

namespace DemoCluster.DAL.Database.Runtime
{
    public class RuntimeContext
    {
        private readonly IMongoDatabase database;

        public RuntimeContext(MongoDbOptions options)
        {
            
        }
    }
}