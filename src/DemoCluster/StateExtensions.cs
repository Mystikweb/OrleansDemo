using DemoCluster.Models;
using DemoCluster.States;
using System;

namespace DemoCluster
{
    public static class StateExtensions
    {
        public static CurrentDeviceStateViewModel ToViewModel(this CurrentDeviceState state, Guid deviceId) => new CurrentDeviceStateViewModel
        {
            DeviceId = deviceId,
            DeviceStateId = state.DeviceStateId,
            StateId = state.StateId,
            StateName = state.Name,
            Timestamp = state.Timestamp
        };

        public static SensorSummaryViewModel ToViewModel(this SensorState state) => new SensorSummaryViewModel
        {
            DeviceSensorId = state.DeviceSensorId,
            SensorId = state.SensorId,
            SensorName = state.Name,
            UOM = state.UOM,
            IsEnabled = state.IsEnabled
        };
    }
}
