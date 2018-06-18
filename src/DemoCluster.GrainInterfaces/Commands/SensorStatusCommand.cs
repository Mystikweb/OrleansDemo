using System;

namespace DemoCluster.GrainInterfaces.Commands
{
    public class SensorStatusCommand : SensorCommand
    {
        public SensorStatusCommand(bool isReceiving, DateTime? timeStamp = null, int? version = null)
            : base(timeStamp, version)
        {
            IsReceiving = isReceiving;
        }

        public bool IsReceiving { get; private set; }
    }
}