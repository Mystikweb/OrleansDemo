using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using OrleansDemo.Models.Runtime;
using OrleansDemo.Server.Services;
using OrleansDemo.Services.Instances;
using OrleansDemo.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OrleansDemo.Server
{
    public class ServerHost
    {
        private ClusterConfiguration clusterConfiguration;
        private ISiloHost siloHost;

        public ServerHost(string configurationXmlFile)
        {
            clusterConfiguration = CreateConfiguration(configurationXmlFile);
        }

        public async Task StartAsync()
        {
            try
            {
                siloHost = BuildSilo();
                await siloHost.StartAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task StopAsync()
        {
            try
            {
                await siloHost.StopAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ClusterConfiguration CreateConfiguration(string configurationXmlFile)
        {
            ClusterConfiguration result = new ClusterConfiguration();

            result.LoadFromFile(configurationXmlFile);

            return result;
        }

        private ISiloHost BuildSilo()
        {
            var builder = new SiloHostBuilder()
                .UseConfiguration(clusterConfiguration)
                .ConfigureSiloName(System.Net.Dns.GetHostName())
                .AddApplicationPartsFromBasePath()
                .ConfigureServices(services =>
                {
                    // hard coding the connection string for now need to come up with a better way
                    services.AddDbContextPool<RuntimeContext>(opts =>
                        opts.UseSqlServer("Server=localhost;Database=OrleansDemoDb;User Id=RuntimeManager;Password=MyPa55w0rd!;MultipleActiveResultSets=True"));

                    services.AddTransient<IConfigurationManager, ConfigurationManager>();
                    services.AddTransient<IRuntimeReading, RuntimeReading>();
                })
                .ConfigureLogging(logging =>
                {
                    if (Environment.UserInteractive)
                    {
                        logging.AddConsole();
                    }
                    else
                    {

                    }
                });

            return builder.Build();
        }
    }
}
