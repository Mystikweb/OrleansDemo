using System;

namespace DemoCluster.Commands
{
    public class UpdateDeviceStatus : BaseCommand
    {
        public UpdateDeviceStatus(bool isEnabled,
            bool shouldRun,
            DateTime? timeStamp = null)
            : base(timeStamp)
        {
            IsEnabled = isEnabled;
            IsRunning = isEnabled && shouldRun ? true : false;
        }

        public bool IsEnabled { get; }
        public bool IsRunning { get; }
    }
}
