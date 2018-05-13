using DemoCluster.DAL.Database.Runtime;
using DemoCluster.DAL.Models;
using System.Linq;

namespace DemoCluster.DAL
{
    public static class ModelExtensions
    {
        public static DeviceStateItem ToDeviceStateItem(this DeviceState state)
        {
            return new DeviceStateItem
            {
                DeviceId = state.DeviceId,
                Name = state.Name,
                IsRunning = state.IsRunning,
                Timestamp = state.Timestamp,
                Sensors = state.Sensors.Select(s => s.ToSensorStateItem()).ToList()
            };
        }

        public static SensorStateItem ToSensorStateItem(this SensorState state)
        {
            return new SensorStateItem
            {
                DeviceSensorId = state.Id,
                Name = state.Name,
                UOM = state.UOM
            };
        }
    }
}