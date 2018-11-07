using System;

namespace DemoCluster.Commands
{
    public class AddSensorValue : BaseCommand
    {
        public AddSensorValue(double value,
            DateTime? timeStamp = null)
            : base(timeStamp)
        {
            Value = value;
        }

        public double Value { get; private set; }
    }
}
