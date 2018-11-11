using DemoCluster.Commands;
using System;
using System.Collections.Generic;

namespace DemoCluster.States
{
    [Serializable]
    public class SensorState
    {
        public int DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsRunning { get; set; }
        public DateTime Timestamp { get; set; }
        public List<SensorValueState> Values { get; set; } = new List<SensorValueState>();

        public void Apply(UpdateSensor @event)
        {
            DeviceSensorId = @event.DeviceSensorId;
            SensorId = @event.SensorId;
            Name = @event.Name;
            UOM = @event.UOM;
            IsEnabled = @event.IsEnabled;
            IsRunning = @event.IsRunning;
            Timestamp = @event.Timestamp;
        }

        public void Apply(UpdateSensorStatus @event)
        {
            IsEnabled = @event.IsEnabled;
            IsRunning = @event.IsRunning;
            Timestamp = @event.Timestamp;
        }

        public void Apply(AddSensorValue @event)
        {
            var stateValue = new SensorValueState
            {
                Value = @event.Value,
                Timestamp = @event.Timestamp
            };

            Values.Add(stateValue);
        }
    }
}
