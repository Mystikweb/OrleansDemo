using DemoCluster.DAL.Database.Configuration;
using DemoCluster.DAL.Logic;
using DemoCluster.GrainImplementations;
using DemoCluster.GrainInterfaces;
using DemoCluster.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoCluster.Hosting
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContextPool<ConfigurationContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));

            services.AddTransient(typeof(DeviceLogic));
            services.AddTransient(typeof(SensorLogic));
            services.AddTransient(typeof(EventTypeLogic));
            services.AddTransient(typeof(StateLogic));

            return services;
        }

        public static IServiceCollection AddDeviceServices(this IServiceCollection services)
        {
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddDeviceServiceClient();

            return services;
        }

        public static IServiceCollection AddDeviceServiceClient(this IServiceCollection services)
        {
            services.AddSingleton<IDeviceServiceClient, DeviceServiceClient>();

            return services;
        }
    }
}
