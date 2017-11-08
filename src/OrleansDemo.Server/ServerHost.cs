using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
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
                .AddApplicationPartsFromBasePath()
                .ConfigureServices(services =>
                {
                    
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
