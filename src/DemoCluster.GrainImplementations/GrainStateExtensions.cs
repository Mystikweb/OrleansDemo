using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using System;
using System.Linq;

namespace DemoCluster.GrainImplementations
{
    public static class GrainStateExtensions
    {
        public static DeviceStateItem ToStateItem(this CurrentDeviceState state, Guid deviceId)
        {
            return new DeviceStateItem
            {
                DeviceId = deviceId,
                DeviceStatusId = state.DeviceStateId,
                StatusId = state.StateId,
                Name = state.Name,
                Timestamp = state.Timestamp
            };
        }

        public static SensorStateSummary ToStateSummary(this SensorState state)
        {
            return new SensorStateSummary
            {
                DeviceSensorId = state.DeviceSensorId,
                SensorId = state.SensorId,
                Name = state.Name,
                UOM = state.UOM,
                IsEnabled = state.IsEnabled
            };
        }
    }
}
