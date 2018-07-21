using System;

namespace DemoCluster.GrainInterfaces.Commands
{
    public class DeviceConfigCommand : DeviceCommand
    {
        public DeviceConfigCommand(Guid deviceId, string name, DateTime? timeStamp = null) 
            : base(timeStamp)
        {
            DeviceId = deviceId;
            Name = name;
        }

        public Guid DeviceId { get; private set; }
        public string Name { get; private set; }
    }
}