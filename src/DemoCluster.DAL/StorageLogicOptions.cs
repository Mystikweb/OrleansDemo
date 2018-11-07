namespace DemoCluster.DAL
{
    public class StorageLogicOptions
    {
        public const string SECTION_NAME = "DemoCluster.StorageLogicOptions";

        public string SqlConfigurationConnectionString { get; set; }
    }
}