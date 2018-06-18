using System;

namespace DemoCluster.GrainInterfaces.Commands
{
    public abstract class SensorCommand
    {
        public SensorCommand(DateTime? timeStamp, int? version)
        {
            this.Timestamp = timeStamp.HasValue ? timeStamp.Value : DateTime.UtcNow;
            this.Version = version.HasValue ? version.Value : 1;
        }

        public int DeviceSensorId { get; set; }
        public DateTime Timestamp { get; private set; }
        public int? Version { get; private set; }
    }
}