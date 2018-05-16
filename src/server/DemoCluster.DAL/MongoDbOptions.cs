namespace DemoCluster.DAL
{
    public class MongoDbOptions
    {
        public const string SECTION_NAME = "MongoDbSettings";
        
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string DeviceHistoryCollection { get; set; }
        public string SensorStateCollection { get; set; }
    }
}