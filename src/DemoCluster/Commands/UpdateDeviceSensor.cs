using System;

namespace DemoCluster.Commands
{
    public class UpdateDeviceSensor : BaseCommand
    {
        public UpdateDeviceSensor(int deviceSensorId, 
            int sensorId, 
            string name, 
            string uom, 
            bool isEnabled, 
            double? lastValue, 
            DateTime? lastValueReceived, 
            double? averageValue, 
            double? totalValue, 
            DateTime? timeStamp = null)
            : base(timeStamp)
        {
            DeviceSensorId = deviceSensorId;
            SensorId = sensorId;
            Name = name;
            UOM = uom;
            IsEnabled = isEnabled;
            LastValue = lastValue;
            LastValueReceived = lastValueReceived;
            AverageValue = averageValue;
            TotalValue = totalValue;
        }

        public int DeviceSensorId { get; private set; }
        public int SensorId { get; private set; }
        public string Name { get; private set; }
        public string UOM { get; private set; }
        public bool IsEnabled { get; private set; }
        public double? LastValue { get; private set; }
        public DateTime? LastValueReceived { get; private set; }
        public double? AverageValue { get; private set; }
        public double? TotalValue { get; private set; }
    }
}
