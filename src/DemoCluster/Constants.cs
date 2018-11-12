namespace DemoCluster
{
    public class Constants
    {
        // SignalR Hub action names
        public const string MESSAGING_REGISTRATION_RECEIVE_CONFIG = "ReceiveConfig";
        public const string MESSAGING_DEVICE_SUMMARY = "DeviceSummary";
        public const string MESSAGING_DEVICE_ERROR = "DeviceError";
        public const string MESSAGING_SENSOR_VALUE_CURRENT_STATE = "CurrentState";

        // reminder names
        public const string REMINDER_GET_SENSOR_UPDATES = "GetSensorUpdates";

        // streaming provider names and namespaces
        public const string SENSOR_STREAM_PROVIDER = "SensorStream";
        public const string SENSOR_STREAM_NAMESPACE = "SensorValues";

        // include propeties for various entity types
        public static readonly string[] DEVICE_PROPERTIES = { "DeviceEventType", "DeviceEventType.EventType", "DeviceState", "DeviceState.State", "DeviceSensor", "DeviceSensor.Sensor" };
    }
}
