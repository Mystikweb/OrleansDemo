using System;
using System.IO;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Orleans.Providers;
using Orleans.Runtime;
using Swashbuckle.AspNetCore.Swagger;

namespace DemoCluster.Api
{
    public class DemoClusterApi : IBootstrapProvider
    {
        private IWebHost host;
        private ILogger logger;

        public string Name { get; private set; }

        public async Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;
            logger = providerRuntime.ServiceProvider.GetRequiredService<ILogger<DemoClusterApi>>();

            string runtimeConnectionString = config.Properties[DemoClusterApiConstants.DEMOCLUSTER_RUNTIME_CONNNECTIONSTRING];

            if (string.IsNullOrEmpty(runtimeConnectionString))
            {
                throw new ApplicationException("Configuration or runtime connection string not provided");
            }

            string hostName = ConfigurationExists(config, DemoClusterApiConstants.DEMOCLUSTER_API_HOSTNAME) ? config.Properties[DemoClusterApiConstants.DEMOCLUSTER_API_HOSTNAME] : "*";
            int port = ConfigurationExists(config, DemoClusterApiConstants.DEMOCLUSTER_API_PORT) ? Convert.ToInt32(config.Properties[DemoClusterApiConstants.DEMOCLUSTER_API_PORT]) : 5000;

            string listeningUri = $"http://{hostName}:{port}";

            try
            {
                //host = WebHost.CreateDefaultBuilder()
                host = new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureServices(services =>
                    {
                        
                        services.AddDbContext<RuntimeContext>(opts =>
                            opts.UseSqlServer(runtimeConnectionString));

                        services.AddTransient<IConfigurationStorage, ConfigurationStorage>();
                        services.AddSingleton(providerRuntime.GrainFactory);
                        services.AddMvc();

                        // Register the Swagger generator, defining one or more Swagger documents
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new Info { Title = "DemoCluster API", Version = "v1" });
                        });
                    })
                    .Configure(app =>
                    {
                        // Enable middleware to serve generated Swagger as a JSON endpoint.
                        app.UseSwagger();

                        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                        // app.UseSwaggerUI(c =>
                        // {
                        //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoCluster API V1");
                        // });

                        app.UseCors(pol =>
                        {
                            pol.AllowAnyOrigin();
                            pol.AllowAnyHeader();
                            pol.AllowAnyMethod();
                        });

                        app.UseMvc();
                    })
                    .UseKestrel()
                    .UseUrls(listeningUri)
                    .Build();

                await host.StartAsync();

                logger.LogInformation($"DemoCluster API listening on {listeningUri}");
            }
            catch (Exception ex)
            {
                logger.LogError(new EventId(1001), ex, ex.Message);
            }
        }

        public async Task Close()
        {
            try
            {
                await host?.StopAsync();
                host?.Dispose();
            }
            catch
            {

            }
        }

        private bool ConfigurationExists(IProviderConfiguration config, string key)
        {
            return config.Properties.ContainsKey(key) && !string.IsNullOrEmpty(config.Properties[key]);
        }
    }
}
