using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.Commands;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceState
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public CurrentDeviceState CurrentState { get; set; } = new CurrentDeviceState();
        public List<DeviceSensorState> Sensors { get; set; } = new List<DeviceSensorState>();

        public void Apply(DeviceConfigCommand @event)
        {
            DeviceId = @event.DeviceId;
            Name = @event.Name;
            Timestamp = @event.Timestamp;
        }

        public void Apply(DeviceStatusCommand @event)
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
    }

    [Serializable]
    public class CurrentDeviceState
    {
        public int DeviceStateId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
    }

    [Serializable]
    public class DeviceSensorState
    {
        public int DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool Enabled { get; set; }
        public double? AverageValue { get; set; }
        public double? TotalValue { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
