using System;

namespace DemoCluster
{
    public class Constants
    {
        // streaming provider names and namespaces
        public const string SENSOR_STREAM_PROVIDER = "SensorStream";
        public const string SENSOR_STREAM_NAMESPACE = "SensorValues";

        // include propeties for various entity types
        public static readonly string[] DEVICE_PROPERTIES = { "DeviceEventType", "DeviceEventType.EventType", "DeviceState", "DeviceState.State", "DeviceSensor", "DeviceSensor.Sensor" };
    }
}
