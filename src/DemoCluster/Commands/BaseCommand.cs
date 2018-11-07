using System;

namespace DemoCluster.Commands
{
    public abstract class BaseCommand
    {
        public BaseCommand(DateTime? timeStamp)
        {
            Timestamp = timeStamp ?? DateTime.UtcNow;
        }
        public DateTime Timestamp { get; private set; }
    }
}
