using DemoCluster.DAL.Database;
using DemoCluster.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.DAL
{
    public static class DemoClusterDALExtensions
    {
        public static ISiloHostBuilder RegisterStorageLogic(this ISiloHostBuilder builder, string runtimeConnectionString)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContextPool<RuntimeContext>(opts =>
                    opts.UseSqlServer(runtimeConnectionString));

                services.AddTransient<IRuntimeStorage, RuntimeStorage>();
                services.AddTransient<IConfigurationStorage, ConfigurationStorage>();
            });

            return builder;
        }
    }
}
