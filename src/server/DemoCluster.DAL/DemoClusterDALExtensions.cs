using DemoCluster.DAL.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;

namespace DemoCluster.DAL
{
    public static class DemoClusterDALExtensions
    {
        public static ISiloHostBuilder RegisterStorageLogic(this ISiloHostBuilder builder, string runtimeConnectionString)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContextPool<RuntimeContext>(opts =>
                    opts.UseSqlServer(runtimeConnectionString));

                services.AddTransient<IRuntimeStorage, RuntimeStorage>();
                services.AddTransient<IConfigurationStorage, ConfigurationStorage>();
            });

            return builder;
        }

        public static IWebHostBuilder RegisterStorageLogic(this IWebHostBuilder builder, string runtimeConnectionString)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContextPool<RuntimeContext>(opts =>
                    opts.UseSqlServer(runtimeConnectionString));

                services.AddTransient<IRuntimeStorage, RuntimeStorage>();
                services.AddTransient<IConfigurationStorage, ConfigurationStorage>();
            });

            return builder;
        }
    }
}
