using System;

namespace DemoCluster.Commands
{
    public class UpdateSensorStatus : BaseCommand
    {
        public UpdateSensorStatus(bool isEnabled, 
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
