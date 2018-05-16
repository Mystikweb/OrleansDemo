using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DemoCluster.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Runtime;
using Swashbuckle.AspNetCore.Swagger;

namespace DemoCluster.Api
{
    public class DemoClusterApi : IStartupTask, IDisposable
    {
        private readonly DemoClusterApiOptions options;
        private readonly MongoDbOptions mongoOptions;
        private readonly ILogger logger;
        private readonly IGrainFactory grainFactory;

        private IWebHost host;

        public DemoClusterApi(IOptions<DemoClusterApiOptions> options, MongoDbOptions mongoOptions, ILogger<DemoClusterApi> logger, IGrainFactory grainFactory)
        {
            this.options = options.Value;
            this.mongoOptions = mongoOptions;
            this.logger = logger;
            this.grainFactory = grainFactory;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            string runtimeConnectionString = options.RuntimeConnnectionString;

            if (string.IsNullOrEmpty(runtimeConnectionString))
            {
                throw new ApplicationException("Configuration or runtime connection string not provided");
            }

            string listeningUri = $"http://{options.HostName}:{options.Port}";

            try
            {
                //host = WebHost.CreateDefaultBuilder()
                host = new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .RegisterStorageLogic(runtimeConnectionString, mongoOptions)
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton(grainFactory);
                        services.AddSingleton<IActionDispatcher>(new ActionDispatcher(TaskScheduler.Current));
                        services.AddSingleton(logger);
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

                logger.Info($"DemoCluster API listening on {listeningUri}");
            }
            catch (Exception ex)
            {
                logger.Error(1001, ex.Message, ex);
            }
        }

        public void Dispose()
        {
            host?.Dispose();
        }
    }
}
