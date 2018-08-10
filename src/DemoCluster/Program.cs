using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
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
            
            var storageLogicOptions = appConfig.GetSection(StorageLogicOptions.SECTION_NAME).Get<StorageLogicOptions>();
            var redisOptions = appConfig.GetSection(RedisProviderOptions.SECTION_NAME).Get<RedisProviderOptions>();
            var rabbitOptions = appConfig.GetSection(RabbitMessagingOptions.SECTION_NAME).Get<RabbitMessagingOptions>();

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
                .AddRedisGrainStorage("CacheStorage", options =>
                {
                    options.Hostname = redisOptions.Hostname;
                    options.Port = redisOptions.Port;
                    options.Password = redisOptions.Password;
                    options.UseJsonFormat = redisOptions.UseJsonFormat;
                    options.DatabaseNumber = 1;
                })
                .AddMemoryGrainStorage("MemoryStorage")
                .AddLogStorageBasedLogConsistencyProvider()
                .UseStorageLogic(storageLogicOptions)
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
                .AddStartupTask<DeviceRegistryStartup>()
                .ConfigureLogging(log => log.AddConsole())
                .UseDashboard();                

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}