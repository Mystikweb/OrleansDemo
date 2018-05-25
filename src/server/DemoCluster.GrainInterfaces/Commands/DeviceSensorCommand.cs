using DemoCluster.GrainInterfaces.States;

namespace DemoCluster.GrainInterfaces.Commands
{
    public class DeviceSensorAddCommand : DeviceCommand
    {
        public DeviceSensorState SensorState { get; set; }
    }

    public class DeviceSensorRemoveCommand : DeviceCommand
    {
        public DeviceSensorState SensorState { get; set; }
    }
}