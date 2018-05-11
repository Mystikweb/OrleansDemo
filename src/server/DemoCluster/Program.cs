using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DemoCluster.Api;
using DemoCluster.DAL;
using DemoCluster.GrainImplementations;
using DemoCluster.Util;
using DemoCluster.Util.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Storage.Redis;

namespace DemoCluster
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        public static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press [Enter] to terminate...");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var clusterConnectionString = appConfig.GetConnectionString("Cluster");
            var runtimeConnectionString = appConfig.GetConnectionString("Runtime");

            var rabbitOptions = appConfig.GetSection(RabbitMessagingOptions.SECTION_NAME).Get<RabbitMessagingOptions>();
            var redisOptions = appConfig.GetSection(RedisProviderOptions.SECTION_NAME).Get<RedisProviderOptions>();

            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = appConfig["ClusterId"];
                    options.ServiceId = appConfig["ServiceId"];
                })
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = clusterConnectionString;
                    options.Invariant = "System.Data.SqlClient";
                })
                .UseAdoNetReminderService(options =>
                {
                    options.ConnectionString = clusterConnectionString;
                    options.Invariant = "System.Data.SqlClient";
                })
                .AddAdoNetGrainStorage("SqlBase", options =>
                {
                    options.ConnectionString = clusterConnectionString;
                    options.Invariant = "System.Data.SqlClient";
                    options.UseJsonFormat = true;
                })
                .AddMemoryGrainStorage("MemoryStorage")
                .AddMemoryGrainStorage("PubSubStore")
                .AddCustomStorageBasedLogConsistencyProvider("CustomStorage")
                .AddSimpleMessageStreamProvider("PubSub")
                .UseDashboard()
                .UseStorageLogic(runtimeConnectionString: runtimeConnectionString)
                .UseRabbitMessaging(options: rabbitOptions)
                .UseApi(options =>
                {
                    options.RuntimeConnnectionString = runtimeConnectionString;
                })
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
                .ConfigureLogging(log => log.AddConsole())
                .AddStartupTask<DeviceRegistryBootstrap>();

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
