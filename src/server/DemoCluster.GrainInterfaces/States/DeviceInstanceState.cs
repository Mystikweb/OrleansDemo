using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.Commands;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceInstanceState
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public DeviceStatusState CurrentStatus { get; set; }
        public List<DeviceSensorState> Sensors { get; set; } = new List<DeviceSensorState>();

        public void Apply(DeviceStatusCommand statusUpdate)
        {
            this.CurrentStatus = statusUpdate.NewState;
        }

        public void Apply(DeviceSensorAddCommand addSensorUpdate)
        {
            this.Sensors.Add(addSensorUpdate.SensorState);
        }

        public void Apply(DeviceSensorRemoveCommand removeSensorUpdate)
        {
            this.Sensors.Remove(removeSensorUpdate.SensorState);
        }
    }
}