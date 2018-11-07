using DemoCluster.Commands;
using System;
using System.Collections.Generic;

namespace DemoCluster.States
{
    [Serializable]
    public class DeviceState
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public CurrentDeviceState CurrentState { get; set; } = new CurrentDeviceState();
        public List<DeviceSensorState> Sensors { get; set; } = new List<DeviceSensorState>();

        public void Apply(UpdateDevice @event)
        {
            DeviceId = @event.DeviceId;
            Name = @event.Name;
            Timestamp = @event.Timestamp;
        }

        public void Apply(UpdateDeviceState @event)
        {
            var newState = new CurrentDeviceState
            {
                DeviceStateId = @event.DeviceStateId,
                StateId = @event.StatusId,
                Name = @event.Name,
                Timestamp = @event.Timestamp
            };

            CurrentState = newState;
        }

        public void Apply(UpdateDeviceSensor @event)
        {
            var newState = new DeviceSensorState
            {
                DeviceSensorId = @event.DeviceSensorId,
                SensorId = @event.SensorId,
                Name = @event.Name,
                UOM = @event.UOM,
                Enabled = @event.IsEnabled,
                LastValue = @event.LastValue,
                LastValueReceived = @event.LastValueReceived,
                AverageValue = @event.AverageValue,
                TotalValue = @event.TotalValue,
                Timestamp = @event.Timestamp
            };

            if (Sensors.Exists(s => s.DeviceSensorId == newState.DeviceSensorId))
            {
                int idx = Sensors.IndexOf(Sensors.Find(s => s.DeviceSensorId == newState.DeviceSensorId));
                Sensors[idx] = newState;
            }
            else
            {
                Sensors.Add(newState);
            }
        }
    }
}
