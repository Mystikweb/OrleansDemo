using DemoCluster.DAL.Database.Configuration;
using DemoCluster.Models;
using System;
using System.Linq;

namespace DemoCluster.DAL
{
    public static class ModelExtensions
    {
        public static DeviceViewModel ToViewModel(this Device model) => new DeviceViewModel
        {
            DeviceId = model.DeviceId.ToString(),
            Name = model.Name,
            IsEnabled = model.IsEnabled,
            Sensors = model.DeviceSensor.Select(s => s.ToViewModel()).ToList(),
            EventTypes = model.DeviceEventType.Select(e => e.ToViewModel()).ToList(),
            States = model.DeviceState.Select(t => t.ToViewModel()).ToList()
        };

        public static Device ToModel(this DeviceViewModel model) => new Device
        {
            DeviceId = Guid.Parse(model.DeviceId),
            Name = model.Name,
            IsEnabled = model.IsEnabled
        };

        public static DeviceSensorViewModel ToViewModel(this DeviceSensor model) => new DeviceSensorViewModel
        {
            DeviceSensorId = model.DeviceSensorId,
            DeviceId = model.DeviceId.ToString(),
            SensorId = model.SensorId,
            SensorName = model.Sensor.Name,
            UOM = model.Sensor.Uom,
            IsEnabled = model.IsEnabled
        };

        public static DeviceSensor ToModel(this DeviceSensorViewModel model) => new DeviceSensor
        {
            DeviceSensorId = model.DeviceSensorId.GetValueOrDefault(),
            DeviceId = Guid.Parse(model.DeviceId),
            SensorId = model.SensorId,
            IsEnabled = model.IsEnabled
        };

        public static DeviceEventTypeViewModel ToViewModel(this DeviceEventType model) => new DeviceEventTypeViewModel
        {
            DeviceEventTypeId = model.DeviceEventTypeId,
            DeviceId = model.DeviceId.ToString(),
            EventTypeId = model.EventTypeId,
            EventTypeName = model.EventType.Name,
            IsEnabled = model.IsEnabled
        };

        public static DeviceEventType ToModel(this DeviceEventTypeViewModel model) => new DeviceEventType
        {
            DeviceEventTypeId = model.DeviceEventTypeId.GetValueOrDefault(),
            DeviceId = Guid.Parse(model.DeviceId),
            EventTypeId = model.EventTypeId,
            IsEnabled = model.IsEnabled
        };

        public static DeviceStateViewModel ToViewModel(this DeviceState model) => new DeviceStateViewModel
        {
            DeviceStateId = model.DeviceStateId,
            DeviceId = model.DeviceId.ToString(),
            StateId = model.StateId,
            StateName = model.State.Name,
            IsEnabled = model.IsEnabled
        };

        public static DeviceState ToModel(this DeviceStateViewModel model) => new DeviceState
        {
            DeviceStateId = model.DeviceStateId.GetValueOrDefault(),
            DeviceId = Guid.Parse(model.DeviceId),
            StateId = model.StateId,
            IsEnabled = model.IsEnabled
        };

        public static SensorViewModel ToViewModel(this Sensor model) => new SensorViewModel
        {
            SensorId = model.SensorId,
            Name = model.Name,
            Uom = model.Uom
        };

        public static Sensor ToModel(this SensorViewModel model) => new Sensor
        {
            SensorId = model.SensorId.GetValueOrDefault(),
            Name = model.Name,
            Uom = model.Uom
        };

        public static EventTypeViewModel ToViewModel(this EventType model) => new EventTypeViewModel
        {
            EventId = model.EventTypeId,
            Name = model.Name
        };

        public static EventType ToModel(this EventTypeViewModel model) => new EventType
        {
            EventTypeId = model.EventId.GetValueOrDefault(),
            Name = model.Name
        };

        public static StateViewModel ToViewModel(this State model) => new StateViewModel
        {
            StateId = model.StateId,
            Name = model.Name
        };

        public static State ToModel(this StateViewModel model) => new State
        {
            StateId = model.StateId.GetValueOrDefault(),
            Name = model.Name
        };
    }
}
