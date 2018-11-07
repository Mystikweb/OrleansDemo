using System;

namespace DemoCluster.Commands
{
    public class UpdateSensorStatus : BaseCommand
    {
        public UpdateSensorStatus(bool isEnabled, 
            DateTime? timeStamp = null)
            : base(timeStamp)
        {
            IsEnabled = isEnabled;
        }

        public bool IsEnabled { get; private set; }
    }
}
