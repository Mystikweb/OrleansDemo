using System;

namespace DemoCluster.Commands
{
    public class UpdateSensor : BaseCommand
    {
        public UpdateSensor(int deviceSensorId,
            int sensorId, 
            string name, 
            string uom, 
            bool isEnabled,
            bool shouldRun,
            DateTime? timeStamp = null)
            : base(timeStamp)
        {
            DeviceSensorId = deviceSensorId;
            SensorId = sensorId;
            Name = name;
            UOM = uom;
            IsEnabled = isEnabled;
            IsRunning = isEnabled && shouldRun ? true : false;
        }

        public int DeviceSensorId { get; }
        public int SensorId { get; }
        public string Name { get; }
        public string UOM { get; }
        public bool IsEnabled { get; }
        public bool IsRunning { get; }
    }
}
