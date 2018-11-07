using DemoCluster.Hosting;
using DemoCluster.GrainImplementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Storage.Redis;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DemoCluster
{
    class Program
    {
        static readonly ManualResetEvent _siloStopped = new ManualResetEvent(false);

        static ISiloHost silo;
        static bool siloStopping = false;
        static readonly object syncLock = new object();

        static void Main(string[] args)
        {
            SetupApplicationShutdown();

            silo = CreateSilo();
            silo.StartAsync().Wait();

            /// Wait for the silo to completely shutdown before exiting. 
            _siloStopped.WaitOne();
        }

        static void SetupApplicationShutdown()
        {
            /// Capture the user pressing Ctrl+C
            Console.CancelKeyPress += (s, a) => {
                /// Prevent the application from crashing ungracefully.
                a.Cancel = true;
                /// Don't allow the following code to repeat if the user presses Ctrl+C repeatedly.
                lock (syncLock)
                {
                    if (!siloStopping)
                    {
                        siloStopping = true;
                        Task.Run(StopSilo).Ignore();
                    }
                }
                /// Event handler execution exits immediately, leaving the silo shutdown running on a background thread,
                /// but the app doesn't crash because a.Cancel has been set = true
            };
        }

        static async Task StopSilo()
        {
            await silo.StopAsync();
            _siloStopped.Set();
        }

        private static ISiloHost CreateSilo()
        {
            var environmentConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            string environmentName = environmentConfig.GetValue<string>("ENVIRONMENT");

            var appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .Build();

            return new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .Configure<ClusterOptions>(options => appConfig.GetSection("ClusterOptions").Bind(options))
                .Configure<ProcessExitHandlingOptions>(options => options.FastKillOnProcessExit = false)
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = appConfig.GetConnectionString("Cluster");
                    options.Invariant = "System.Data.SqlClient";
                })
                .UseAdoNetReminderService(options =>
                {
                    options.ConnectionString = appConfig.GetConnectionString("Cluster");
                    options.Invariant = "System.Data.SqlClient";
                })
                .AddAdoNetGrainStorage("SqlBase", options =>
                {
                    options.ConnectionString = appConfig.GetConnectionString("Cluster");
                    options.Invariant = "System.Data.SqlClient";
                    options.UseJsonFormat = true;
                })
                .AddMemoryGrainStorage("MemoryStorage")
                .AddLogStorageBasedLogConsistencyProvider()
                .AddRedisGrainStorage("CacheStorage", options => appConfig.GetSection(RedisProviderOptions.SECTION_NAME).Bind(options))
                .AddSimpleMessageStreamProvider("SensorValues")
                .AddDataAccess(appConfig.GetConnectionString("Configuration"))
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
                .ConfigureLogging(log =>
                {
                    log.AddConfiguration(appConfig.GetSection("Logging"));
                    log.AddConsole();
                    log.AddDebug();
                })
                .UseDashboard()
                .AddStartupTask<DeviceRegistryStartup>()
                .Build();
        }
    }
}