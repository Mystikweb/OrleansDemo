using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Runtime.Configuration;
using Microsoft.EntityFrameworkCore;
using OrleansDemo.Models.Configuration;
using OrleansDemo.Services.Interfaces;
using OrleansDemo.Services.Instances;

namespace OrleansDemo.API
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
            services.AddDbContext<ConfigurationContext>(opts =>
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IReadingTypeConfiguration, ReadingTypeConfiguration>();
            services.AddTransient<IDeviceTypeConfiguration, DeviceTypeConfiguration>();

            services.AddSingleton(BuildClient);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(pol =>
            {
                pol.AllowAnyOrigin();
                pol.AllowAnyHeader();
                pol.AllowAnyMethod();
            });

            app.UseMvc();
        }

        private IClusterClient BuildClient(IServiceProvider arg)
        {
            ClientConfiguration configuration = ClientConfiguration.LoadFromFile("ClientConfiguration.xml");
            return new ClientBuilder()
                .UseConfiguration(configuration)
                .AddApplicationPartsFromBasePath()
                .ConfigureLogging(logger =>
                {
                    
                }).Build();
        }
    }
}
