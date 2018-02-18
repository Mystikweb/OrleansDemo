using System;
using System.IO;
using System.Threading.Tasks;
using DemoCluster.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Providers.RabbitMQ.Streams;
using Orleans.Runtime.Configuration;
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

            var connectionString = appConfig.GetConnectionString("Cluster");
            var rabbitOptions = appConfig.GetSection(RabbitMQStreamProviderOptions.SECTION_NAME).Get<RabbitMQStreamProviderOptions>();
            var redisOptions = appConfig.GetSection(RedisProviderOptions.SECTION_NAME).Get<RedisProviderOptions>();

            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.Globals.ClusterId = appConfig["ClusterId"];

            config.Globals.AdoInvariant = "System.Data.SqlClient";
            config.Globals.DataConnectionString = connectionString;
            config.Globals.LivenessEnabled = true;
            config.Globals.LivenessType = GlobalConfiguration.LivenessProviderType.SqlServer;

            config.Globals.AdoInvariantForReminders = "System.Data.SqlClient";
            config.Globals.DataConnectionStringForReminders = connectionString;
            config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.SqlServer;

            config.AddMemoryStorageProvider("PubSubStore");
            config.AddAdoNetStorageProvider("SqlBase", connectionString, AdoNetSerializationFormat.Json);
            config.AddRedisStorageProvider("RedisBase", redisOptions);

            config.AddRabbitMQStreamProvider("Rabbit");

            config.RegisterDashboard();
            config.RegisterApi();

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())                
                .ConfigureLogging(log => log.AddConsole())
                .UseSqlMembership(opts => opts.Configure(mbr =>
                {
                    mbr.ConnectionString = connectionString;
                    mbr.AdoInvariant = "System.Data.SqlClient";

                }))
                .ConfigureRabbitMQStreamProvider(rabbitOptions)
                .UseDashboard(options =>
                {
                    options.HostSelf = true;
                    options.HideTrace = false;
                })
                .UseApi();

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
