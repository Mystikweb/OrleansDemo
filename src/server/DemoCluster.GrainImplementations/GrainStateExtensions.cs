using DemoCluster.DAL.Database.Runtime;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.Commands;
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

        public static DeviceStateCommand ToDeviceStateCommand(this DeviceInstanceHistory history)
        {
            return new DeviceStateCommand
            {
                DeviceId = Guid.Parse(history.DeviceId),
                Name = history.Name,
                CurrentStatus = history.Status.ToDeviceStatusState(),
                Sensors = history.Sensors.Select(s => s.ToDeviceSensorState()).ToList()
            };
        }

        public static DeviceStatusState ToDeviceStatusState(this DeviceStatus status)
        {
            return new DeviceStatusState
            {
                DeviceStateId = status.DeviceStateId,
                Name = status.Name
            };
        }

        public static DeviceSensorState ToDeviceSensorState(this SensorStatus status)
        {
            return new DeviceSensorState
            {
                DeviceSensorId = status.DeviceSensorId,
                Name = status.Name,
                UOM = status.UOM,
                IsReceiving = status.IsReceiving
            };
        }

        public static DeviceInstanceHistory ToDeviceInstanceHistory(this DeviceInstanceState state)
        {
            return new DeviceInstanceHistory
            {
                DeviceId = state.DeviceId.ToString(),
                Name = state.Name,
                Status = state.CurrentStatus.ToDeviceStatus(),
                Sensors = state.Sensors.Select(s => s.ToSensorStatus()).ToList()
            };
        }

        public static DeviceStatus ToDeviceStatus(this DeviceStatusState state)
        {
            return new DeviceStatus
            {
                DeviceStateId = state.DeviceStateId,
                Name = state.Name
            };
        }

        public static SensorStatus ToSensorStatus(this DeviceSensorState state)
        {
            return new SensorStatus
            {
                DeviceSensorId = state.DeviceSensorId,
                Name = state.Name,
                UOM = state.UOM,
                IsReceiving = state.IsReceiving
            };
        }

        // public static DeviceHistoryState ToDeviceHistoryState(this DeviceHistoryItem item)
        // {
        //     return new DeviceHistoryState
        //     {
        //         DeviceId = Guid.Parse(item.DeviceId),
        //         Name = item.Name,
        //         Timestamp = DateTimeOffset.FromUnixTimeSeconds(item.Timestamp).DateTime,
        //         IsRunning = item.IsRunning,
        //         EventTypeCount = 0,
        //         SensorStatus = item.Sensors.Select(s => s.ToSensoryStatusState()).ToList()
        //     };
        // }

        // public static SensorStatusState ToSensoryStatusState(this SensorStatusItem item)
        // {
        //     return new SensorStatusState
        //     {
        //         Id = item.DeviceSensorId,
        //         Name = item.Name,
        //         UOM = item.UOM,
        //         IsReceiving = item.IsReceiving
        //     };
        // }

        // public static DeviceHistoryItem ToDeviceHistoryItem(this DeviceHistoryState state)
        // {
        //     DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //     return new DeviceHistoryItem
        //     {
        //         DeviceId = state.DeviceId.ToString(),
        //         Name = state.Name,
        //         IsRunning = state.IsRunning,
        //         Timestamp = (long)(state.Timestamp - sTime).TotalSeconds,
        //         Sensors = state.SensorStatus.Select(s => s.ToSensorStatusItem()).ToList()
        //     };
        // }

        // public static SensorStatusItem ToSensorStatusItem(this SensorStatusState state)
        // {
        //     return new SensorStatusItem
        //     {
        //         DeviceSensorId = state.Id,
        //         Name = state.Name,
        //         UOM = state.UOM,
        //         IsReceiving = state.IsReceiving
        //     };
        // }
    }
}
