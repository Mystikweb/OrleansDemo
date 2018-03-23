using DemoCluster.DAL.Database;
using DemoCluster.DAL.Models;
using DemoCluster.DAL.States;
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

        public static DeviceState ToState(this DeviceStateItem item)
        {
            return new DeviceState
            {
                DeviceId = Guid.Parse(item.DeviceId),
                Name = item.Name
            };
        }

        public static DeviceHistoryState ToState(this DeviceHistoryItem item)
        {
            return new DeviceHistoryState
            {
                DeviceId = item.DeviceId,
                Timestamp = item.TimeStamp,
                IsRunning = item.IsRunning,
                SensorCount = item.SensorCount,
                EventTypeCount = item.EventTypeCount
            };
        }

        public static DeviceHistoryItem ToItem(this DeviceHistoryState state, string name)
        {
            return new DeviceHistoryItem
            {
                DeviceId = state.DeviceId,
                Name = name,
                IsRunning = state.IsRunning,
                TimeStamp = state.Timestamp,
                SensorCount = state.SensorCount,
                EventTypeCount = state.EventTypeCount
            };
        }
    }
}
