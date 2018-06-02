using System;
using DemoCluster.DAL.Database;
using DemoCluster.DAL.Database.Configuration;
using DemoCluster.DAL.Database.Runtime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Orleans.Hosting;

namespace DemoCluster.DAL
{
    public static class DemoClusterDALExtensions
    {
        public static ISiloHostBuilder UseStorageLogic(this ISiloHostBuilder builder,
            StorageLogicOptions options)
        {
            return builder.ConfigureServices(services =>
                services.ConfigureStorageLogicServices(options));
        }

        public static IWebHostBuilder UseStorageLogic(this IWebHostBuilder builder,
            StorageLogicOptions options)
        {
            return builder.ConfigureServices(services =>
                services.ConfigureStorageLogicServices(options));
        }

        public static IServiceCollection ConfigureStorageLogicServices(this IServiceCollection services, StorageLogicOptions options)
        {
            services.AddDbContextPool<ConfigurationContext>(dbOptions =>
                dbOptions.UseSqlServer(options.SqlConfigurationConnectionString));

            services.AddSingleton<StorageLogicOptions>(options);
            services.AddSingleton<RuntimeCollections>(options.RuntimeCollections);
            services.AddSingleton<IMongoDatabase>(provider =>
            {
                var connection = new MongoClient($"{options.MongoRuntimeConnectionString}");
                return connection.GetDatabase(options.MongoRuntimeDatabaseName);
            });

            services.AddTransient<IConfigurationStorage, ConfigurationStorage>();
            services.AddTransient<IRuntimeStorage, RuntimeStorage>();

            return services;
        }
    }
}
