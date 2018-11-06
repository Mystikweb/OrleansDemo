using DemoCluster.DAL;
using DemoCluster.GrainInterfaces;
using DemoCluster.Messaging.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace DemoCluster.Messaging
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogicLayer(Configuration.GetConnectionString("Configuration"));
            services.AddSingleton(CreateClusterClient);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSignalR(routes => 
            {
                routes.MapHub<DeviceRegistrationHub>("/registration");
                routes.MapHub<SensorValueHub>("/sensorvalue");
            });

            app.UseMvc();
        }

        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetService<ILogger<Startup>>();

            var client = new ClientBuilder()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IDeviceRegistry).Assembly))
                .Configure<ClusterOptions>(options => Configuration.GetSection("ClusterOptions").Bind(options))
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = Configuration.GetConnectionString("Cluster");
                    options.Invariant = "System.Data.SqlClient";
                })
                .Build();

            client.Connect(RetryFilter).GetAwaiter().GetResult();

            return client;

            async Task<bool> RetryFilter(Exception exception)
            {
                log?.LogWarning("Exception while attempting to connect to Orleans cluster: {Exception}", exception);

                await Task.Delay(TimeSpan.FromSeconds(2));

                return true;
            }
        }
    }
}
