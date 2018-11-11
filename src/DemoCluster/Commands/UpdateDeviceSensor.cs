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
            bool isRunning,
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
            IsRunning = isRunning;
            LastValue = lastValue;
            LastValueReceived = lastValueReceived;
            AverageValue = averageValue;
            TotalValue = totalValue;
        }

        public int DeviceSensorId { get; }
        public int SensorId { get; }
        public string Name { get; }
        public string UOM { get; }
        public bool IsEnabled { get; }
        public bool IsRunning { get; }
        public double? LastValue { get; }
        public DateTime? LastValueReceived { get; }
        public double? AverageValue { get; }
        public double? TotalValue { get; }
    }
}
