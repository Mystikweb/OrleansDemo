using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainImplementations
{
    public static class GrainImplementationExtensions
    {
        public static ClusterConfiguration RegisterBootstrapGrains(this ClusterConfiguration config)
        {
            config.Globals.RegisterBootstrapProvider<DeviceRegistryBootstrap>("RegistryBootstrap");

            return config;
        }

        public static DeviceState ToState(this DeviceConfig item)
        {
            return new DeviceState
            {
                DeviceId = Guid.Parse(item.DeviceId),
                Name = item.Name
            };
        }

        public static SensorState ToSensorState(this DeviceSensorConfig item)
        {
            return new SensorState
            {
                DeviceSensorId = item.DeviceSensorId.Value,
                Name = item.Name,
                UOM = item.UOM
            };
        }

        public static SensorReceiverState ToReceiverState(this DeviceSensorConfig item, string deviceName)
        {
            return new SensorReceiverState
            {
                DeviceSensorId = item.DeviceSensorId.Value,
                Device = deviceName,
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
