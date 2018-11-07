using System;

namespace DemoCluster.Commands
{
    public class UpdateDevice : BaseCommand
    {
        public UpdateDevice(Guid deviceId, string name, DateTime? timeStamp = null)
            : base(timeStamp)
        {
            DeviceId = deviceId;
            Name = name;
        }

        public Guid DeviceId { get; private set; }
        public string Name { get; private set; }
    }
}
