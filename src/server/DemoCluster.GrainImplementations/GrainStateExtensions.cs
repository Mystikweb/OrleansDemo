using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using System;
using System.Linq;

namespace DemoCluster.GrainImplementations
{
    public static class GrainStateExtensions
    {
        public static DeviceState ToDeviceGrainState(this DeviceConfig item)
        {
            return new DeviceState
            {
                DeviceId = Guid.Parse(item.DeviceId),
                Name = item.Name
            };
        }

        public static DeviceHistoryState ToDeviceHistoryState(this DeviceHistoryItem item)
        {
            return new DeviceHistoryState
            {
                DeviceId = Guid.Parse(item.DeviceId),
                Name = item.Name,
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(item.Timestamp).DateTime,
                IsRunning = item.IsRunning,
                EventTypeCount = 0,
                SensorStatus = item.Sensors.Select(s => s.ToSensoryStatusState()).ToList()
            };
        }

        public static SensorStatusState ToSensoryStatusState(this SensorStatusItem item)
        {
            return new SensorStatusState
            {
                Id = item.DeviceSensorId,
                Name = item.Name,
                UOM = item.UOM,
                IsReceiving = item.IsReceiving
            };
        }

        public static DeviceHistoryItem ToDeviceHistoryItem(this DeviceHistoryState state)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return new DeviceHistoryItem
            {
                DeviceId = state.DeviceId.ToString(),
                Name = state.Name,
                IsRunning = state.IsRunning,
                Timestamp = (long)(state.Timestamp - sTime).TotalSeconds,
                Sensors = state.SensorStatus.Select(s => s.ToSensorStatusItem()).ToList()
            };
        }

        public static SensorStatusItem ToSensorStatusItem(this SensorStatusState state)
        {
            return new SensorStatusItem
            {
                DeviceSensorId = state.Id,
                Name = state.Name,
                UOM = state.UOM,
                IsReceiving = state.IsReceiving
            };
        }

        public static SensorState ToSensorState(this DeviceSensorConfig item)
        {
            return new SensorState
            {
                DeviceSensorId = item.DeviceSensorId.Value,
                Name = item.Name,
                UOM = item.UOM,
                IsReceiving = false
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
    }
}
