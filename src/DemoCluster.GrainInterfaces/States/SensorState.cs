using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.Commands;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class SensorState
    {
        public int DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Timestamp { get; set; }
        public List<SensorStateValue> Values { get; set; } = new List<SensorStateValue>();

        public void Apply(SensorConfigCommand @event)
        {
            DeviceSensorId = @event.DeviceSensorId;
            SensorId = @event.SensorId;
            Name = @event.Name;
            UOM = @event.UOM;
            IsEnabled = @event.IsEnabled;
            Timestamp = @event.Timestamp;
        }

        public void Apply(SensorStatusCommand @event)
        {
            IsEnabled = @event.IsEnabled;
            Timestamp = @event.Timestamp;
        }

        public void Apply(SensorValueAddedCommand @event)
        {
            var stateValue = new SensorStateValue
            {
                Value = @event.Value,
                Timestamp = @event.Timestamp
            };

            Values.Add(stateValue);
        }
    }

    [Serializable]
    public class SensorStateValue
    {
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}