using DemoCluster.DAL.Database.Runtime;
using System;
using System.Linq;

namespace DemoCluster.DAL.Models
{
    public static class ModelExtensions
    {
        // public static DeviceHistoryItem ToDeviceHistoryItem(this DeviceHistory state)
        // {
        //     return new DeviceHistoryItem
        //     {
        //         DeviceId = state.DeviceId,
        //         Name = state.Name,
        //         IsRunning = state.IsRunning,
        //         Timestamp = state.Timestamp,
        //         Sensors = state.Sensors.Select(s => s.ToSensorStatusItem()).ToList()
        //     };
        // }

        // public static SensorStatusItem ToSensorStatusItem(this SensorStatus state)
        // {
        //     return new SensorStatusItem
        //     {
        //         DeviceSensorId = state.Id,
        //         Name = state.Name,
        //         UOM = state.UOM
        //     };
        // }

        // public static DeviceHistory ToDeviceHistory(this DeviceHistoryItem item)
        // {
        //     return new DeviceHistory
        //     {
        //         DeviceId = item.DeviceId,
        //         Name = item.Name,
        //         IsRunning = item.IsRunning,
        //         Timestamp = item.Timestamp,
        //         Sensors = item.Sensors.Select(s => s.ToSensorStatus()).ToList()
        //     };
        // }

        // public static SensorStatus ToSensorStatus(this SensorStatusItem item)
        // {
        //     return new SensorStatus
        //     {
        //         Id = item.DeviceSensorId,
        //         Name = item.Name,
        //         UOM = item.UOM,
        //         IsReceiving = item.IsReceiving
        //     };
        // }
    }
}