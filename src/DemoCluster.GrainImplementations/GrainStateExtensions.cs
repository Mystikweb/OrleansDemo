using DemoCluster.DAL.Database.Runtime;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using System;
using System.Linq;

namespace DemoCluster.GrainImplementations
{
    public static class GrainStateExtensions
    {
        public static DeviceState ToDeviceGrainState(this DeviceConfig item)
        {
            return new DeviceState
            {
                DeviceId = Guid.Parse(item.DeviceId),
                Name = item.Name
            };
        }

        public static DeviceStatusCommand CreateDeviceStatusCommand(this DeviceStateHistory item)
        {
            return new DeviceStatusCommand(item.DeviceStateId, 
                item.StateName,
                item.Timestamp,
                item.Version);
        }

        public static DeviceStateHistory CreateDeviceStateHistory(this DeviceStatusCommand item, DeviceStatusState state)
        {
            return new DeviceStateHistory
            {
                DeviceId = state.DeviceId,
                Name = state.Name,
                DeviceStateId = item.DeviceStateId,
                StateName = item.Name,
                Timestamp = item.Timestamp,
                Version = item.Version.Value
            };
        }

        public static SensorStatusCommand CreateSensorStatusCommand(this SensorStateHistory item)
        {
            return new SensorStatusCommand(item.IsReceiving, 
                item.Timestamp, 
                item.Version);
        }

        public static SensorStateHistory CreateSensorStateHistory(this SensorStatusCommand item, SensorStatusState state)
        {
            return new SensorStateHistory
            {
                DeviceSensorId = state.DeviceSensorId,
                DeviceName = state.DeviceName,
                SensorName = state.SensorName,
                Uom = state.Uom,
                IsReceiving = item.IsReceiving,
                Timestamp = item.Timestamp,
                Version = item.Version.Value
            };
        }
    }
}
