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
            DateTime? timeStamp = null)
            : base(timeStamp)
        {
            DeviceSensorId = deviceSensorId;
            SensorId = sensorId;
            Name = name;
            UOM = uom;
            IsEnabled = isEnabled;
        }

        public int DeviceSensorId { get; private set; }
        public int SensorId { get; private set; }
        public string Name { get; private set; }
        public string UOM { get; private set; }
        public bool IsEnabled { get; private set; }
    }
}
