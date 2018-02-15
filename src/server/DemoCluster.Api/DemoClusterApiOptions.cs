
namespace DemoCluster.Api
{
    public sealed class DemoClusterApiOptions
    {
        public bool InternalHost { get; set; } = true;
        public int Port { get; set; } = 5000;
        public string HostName { get; set; } = "*";
    }
}