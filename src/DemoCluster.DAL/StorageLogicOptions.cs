namespace DemoCluster.DAL
{
    public class StorageLogicOptions
    {
        public const string SECTION_NAME = "DemoCluster.StorageLogicOptions";

        public string SqlConfigurationConnectionString { get; set; }
        public string MongoRuntimeConnectionString { get; set; }
        public string MongoRuntimeDatabaseName { get; set; }
        public RuntimeCollections RuntimeCollections { get; set; }
    }

    public class RuntimeCollections
    {
        public string DeviceStatusHistory { get; set; }
    }
}