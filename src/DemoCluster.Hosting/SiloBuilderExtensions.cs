using Orleans.Hosting;

namespace DemoCluster.Hosting
{
    public static class SiloBuilderExtensions
    {
        public static ISiloHostBuilder AddDataAccess(this ISiloHostBuilder builder,
            string connectionString)
        {
            return builder.ConfigureServices(services =>
                services.AddDataAccess(connectionString));
        }
    }
}
