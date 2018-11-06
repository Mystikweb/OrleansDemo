using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DemoCluster.DAL.Database;
using DemoCluster.DAL.Database.Configuration;
using DemoCluster.DAL.Database.Runtime;
using DemoCluster.DAL.Logic;
using DemoCluster.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Orleans.Hosting;

namespace DemoCluster.DAL
{
    public static class UtilityExtensions
    {
        public static TRelated Load<TRelated>(
            this Action<object, string> loader,
            object entity,
            ref TRelated navigationField,
            [CallerMemberName] string navigationName = null)
            where TRelated : class
        {
            loader?.Invoke(entity, navigationName);

            return navigationField;
        }

        public static PaginatedList<T> ToPaginatedList<T>(
            this IEnumerable<T> source,
            int pageIndex,
            int pageSize) where T : class
        {
            var enumerable = source as T[] ?? source.ToArray();
            var count = enumerable.Length;
            var items = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }

    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddLogicLayer(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContextPool<ConfigurationContext>(dbOptions =>
                dbOptions.UseSqlServer(connectionString));

            services.AddRepository();
            services.AddLogic();

            return services;
        }

        public static IServiceCollection AddRepository(
            this IServiceCollection services)
        {
            services.TryAddTransient(
                typeof(IRepository<,>),
                typeof(Repository<,>));

            return services;
        }

        public static IServiceCollection AddLogic(
            this IServiceCollection services)
        {
            services.TryAddTransient(typeof(DeviceLogic));
            services.TryAddTransient(typeof(SensorLogic));
            services.TryAddTransient(typeof(EventLogic));
            services.TryAddTransient(typeof(StateLogic));

            return services;
        }
    }

    public static class ModelExtensions
    {
        public static DeviceConfig ToViewModel(this Device model)
        {
            return new DeviceConfig
            {
                DeviceId = model.DeviceId.ToString(),
                Name = model.Name,
                IsEnabled = model.IsEnabled,
                Sensors = model.DeviceSensor.Select(s => s.ToViewModel()).ToList(),
                Events = model.DeviceEventType.Select(e => e.ToViewModel()).ToList(),
                States = model.DeviceState.Select(t => t.ToViewModel()).ToList()
            };
        }

        public static Device ToModel(this DeviceConfig model)
        {
            return new Device
            {
                DeviceId = Guid.Parse(model.DeviceId),
                Name = model.Name,
                IsEnabled = model.IsEnabled
            };
        }

        public static DeviceSensorConfig ToViewModel(this DeviceSensor model)
        {
            return new DeviceSensorConfig
            {
                DeviceSensorId = model.DeviceSensorId,
                DeviceId = model.DeviceId.ToString(),
                SensorId = model.SensorId,
                Name = model.Sensor.Name,
                UOM = model.Sensor.Uom,
                IsEnabled = model.IsEnabled
            };
        }

        public static DeviceSensor ToModel(this DeviceSensorConfig model)
        {
            return new DeviceSensor
            {
                DeviceSensorId = model.DeviceSensorId.GetValueOrDefault(),
                DeviceId = Guid.Parse(model.DeviceId),
                SensorId = model.SensorId,
                IsEnabled = model.IsEnabled
            };
        }

        public static DeviceEventConfig ToViewModel(this DeviceEventType model)
        {
            return new DeviceEventConfig
            {
                DeviceEventTypeId = model.DeviceEventTypeId,
                DeviceId = model.DeviceId.ToString(),
                EventTypeId = model.EventTypeId,
                Name = model.EventType.Name,
                IsEnabled = model.IsEnabled
            };
        }

        public static DeviceEventType ToModel(this DeviceEventConfig model)
        {
            return new DeviceEventType
            {
                DeviceEventTypeId = model.DeviceEventTypeId.GetValueOrDefault(),
                DeviceId = Guid.Parse(model.DeviceId),
                EventTypeId = model.EventTypeId,
                IsEnabled = model.IsEnabled
            };
        }

        public static DeviceStateConfig ToViewModel(this DeviceState model)
        {
            return new DeviceStateConfig
            {
                DeviceStateId = model.DeviceStateId,
                DeviceId = model.DeviceId.ToString(),
                StateId = model.StateId,
                Name = model.State.Name,
                IsEnabled = model.IsEnabled
            };
        }

        public static DeviceState ToModel(this DeviceStateConfig model)
        {
            return new DeviceState
            {
                DeviceStateId = model.DeviceStateId.GetValueOrDefault(),
                DeviceId = Guid.Parse(model.DeviceId),
                StateId = model.StateId,
                IsEnabled = model.IsEnabled
            };
        }

        public static SensorConfig ToViewModel(this Sensor model)
        {
            return new SensorConfig
            {
                SensorId = model.SensorId,
                Name = model.Name,
                Uom = model.Uom
            };
        }

        public static Sensor ToModel(this SensorConfig model)
        {
            return new Sensor
            {
                SensorId = model.SensorId.GetValueOrDefault(),
                Name = model.Name,
                Uom = model.Uom
            };
        }

        public static EventConfig ToViewModel(this EventType model)
        {
            return new EventConfig
            {
                EventId = model.EventTypeId,
                Name = model.Name
            };
        }

        public static EventType ToModel(this EventConfig model)
        {
            return new EventType
            {
                EventTypeId = model.EventId.GetValueOrDefault(),
                Name = model.Name
            };
        }

        public static StateConfig ToViewModel(this State model)
        {
            return new StateConfig
            {
                StateId = model.StateId,
                Name = model.Name
            };
        }

        public static State ToModel(this StateConfig model)
        {
            return new State
            {
                StateId = model.StateId.GetValueOrDefault(),
                Name = model.Name
            };
        }
    }

    public static class DemoClusterDALExtensions
    {
        public static ISiloHostBuilder AddConfigurationLogic(this ISiloHostBuilder builder,
            string connectionString)
        {
            return builder.ConfigureServices(services =>
                services.AddLogicLayer(connectionString));
        }
    }
}
