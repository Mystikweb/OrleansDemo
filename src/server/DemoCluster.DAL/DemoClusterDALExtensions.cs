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
        public static ISiloHostBuilder UseStorageLogic(this ISiloHostBuilder builder, string connetionString, MongoDbOptions mongoOptions)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContextPool<ConfigurationContext>(opts =>
                    opts.UseSqlServer(connetionString));

                services.AddSingleton<MongoDbOptions>(mongoOptions);
                services.AddSingleton<IMongoDatabase>(provider =>
                {
                    var connection = new MongoClient($"{mongoOptions.ConnectionString}");
                    return connection.GetDatabase(mongoOptions.DatabaseName);
                });

                services.AddTransient<IRuntimeStorage, RuntimeStorage>();
                services.AddTransient<IConfigurationStorage, ConfigurationStorage>();
            });

            return builder;
        }

        public static IWebHostBuilder RegisterStorageLogic(this IWebHostBuilder builder, string connetionString, MongoDbOptions mongoOptions)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContextPool<ConfigurationContext>(opts =>
                    opts.UseSqlServer(connetionString));

                services.AddSingleton<MongoDbOptions>(mongoOptions);
                services.AddSingleton<IMongoDatabase>(provider =>
                {
                    var connection = new MongoClient($"{mongoOptions.ConnectionString}");
                    return connection.GetDatabase(mongoOptions.DatabaseName);
                });

                services.AddTransient<IRuntimeStorage, RuntimeStorage>();
                services.AddTransient<IConfigurationStorage, ConfigurationStorage>();
            });

            return builder;
        }
    }
}
