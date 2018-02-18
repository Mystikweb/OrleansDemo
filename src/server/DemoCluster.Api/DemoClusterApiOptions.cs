
namespace DemoCluster.Api
{
    public sealed class DemoClusterApiOptions
    {
        public int Port { get; set; } = 5000;
        public string HostName { get; set; } = "*";
    }
}