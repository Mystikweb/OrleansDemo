using System;

namespace DemoCluster.GrainInterfaces.Commands
{
    public abstract class SensorCommand
    {
        public SensorCommand(DateTime? timeStamp)
        {
            this.Timestamp = timeStamp.HasValue ? timeStamp.Value : DateTime.UtcNow;
        }

        public DateTime Timestamp { get; private set; }
    }
}