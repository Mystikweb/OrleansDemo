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

        public static DeviceStateHistory CreateDeviceStateHistory(this DeviceStatusCommand item, Guid deviceId, string deviceName)
        {
            return new DeviceStateHistory
            {
                DeviceId = deviceId,
                Name = deviceName,
                DeviceStateId = item.DeviceStateId,
                StateName = item.Name,
                Timestamp = item.Timestamp,
                Version = item.Version.Value
            };
        }
    }
}
