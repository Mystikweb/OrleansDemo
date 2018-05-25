using DemoCluster.GrainInterfaces.States;

namespace DemoCluster.GrainInterfaces.Commands
{
    public class DeviceStatusCommand : DeviceCommand
    {
        public DeviceStatusState NewState { get; set; }
    }
}