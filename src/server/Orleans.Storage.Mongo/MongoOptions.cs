using System;

namespace Orleans.Storage.Mongo
{
    public class MongoOptions
    {
        public const string SECTION_NAME = "Orleans.Storage.Mongo";
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 27017;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
