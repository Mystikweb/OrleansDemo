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

        public static DeviceHistoryState ToState(this DeviceStateItem item)
        {
            return new DeviceHistoryState
            {
                DeviceId = item.DeviceId,
                Timestamp = item.TimeStamp,
                SensorCount = item.SensorCount,
                EventTypeCount = item.EventTypeCount
            };
        }

        public static DeviceStateItem ToItem(this DeviceHistoryState state, string name)
        {
            return new DeviceStateItem
            {
                DeviceId = state.DeviceId,
                Name = name,
                TimeStamp = state.Timestamp,
                SensorCount = state.SensorCount,
                EventTypeCount = state.EventTypeCount
            };
        }
    }
}
