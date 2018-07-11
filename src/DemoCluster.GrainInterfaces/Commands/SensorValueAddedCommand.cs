using System;

namespace DemoCluster.GrainInterfaces.Commands
{
    public class SensorValueAddedCommand : SensorCommand
    {
        public SensorValueAddedCommand(double value, DateTime? timeStamp = null) 
            : base(timeStamp)
        {
            Value = value;
        }

        public double Value { get; private set; }
    }
}