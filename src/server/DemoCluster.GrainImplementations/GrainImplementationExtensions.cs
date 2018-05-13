using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using System;

namespace DemoCluster.GrainImplementations
{
    public static class GrainImplementationExtensions
    {
        public static DeviceState ToDeviceState(this DeviceConfig item)
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
                Timestamp = item.Timestamp,
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
                Timestamp = state.Timestamp,
                SensorCount = state.SensorCount,
                EventTypeCount = state.EventTypeCount
            };
        }
    }
}
