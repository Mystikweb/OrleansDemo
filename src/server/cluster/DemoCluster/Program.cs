using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;

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

            var connectionString = appConfig.GetConnectionString("Default");

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

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                .UseSqlMembership(opts => opts.Configure(mbr =>
                {
                    mbr.ConnectionString = connectionString;
                    mbr.AdoInvariant = "System.Data.SqlClient";

                }))
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
                .ConfigureLogging(log => log.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
