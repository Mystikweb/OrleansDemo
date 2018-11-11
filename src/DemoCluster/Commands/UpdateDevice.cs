using System;

namespace DemoCluster.Commands
{
    public class UpdateDevice : BaseCommand
    {
        public UpdateDevice(Guid deviceId, 
            string name,
            bool isEnabled,
            DateTime? timeStamp = null)
            : base(timeStamp)
        {
            DeviceId = deviceId;
            Name = name;
            IsEnabled = isEnabled;
        }

        public Guid DeviceId { get; }
        public string Name { get; }
        public bool IsEnabled { get; }
    }
}
