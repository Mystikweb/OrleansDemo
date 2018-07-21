using System;

namespace DemoCluster.GrainInterfaces.Commands
{
    public class SensorStatusCommand : SensorCommand
    {
        public SensorStatusCommand(bool isEnabled, DateTime? timeStamp = null)
            : base(timeStamp)
        {
            IsEnabled = isEnabled;
        }

        public bool IsEnabled { get; private set; }
    }
}