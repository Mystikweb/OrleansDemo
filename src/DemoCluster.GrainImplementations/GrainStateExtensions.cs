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

        public static DeviceStateItem ToStateItem(this CurrentDeviceState state, Guid deviceId)
        {
            return new DeviceStateItem
            {
                DeviceId = deviceId,
                DeviceStatusId = state.DeviceStateId,
                StatusId = state.StateId,
                Name = state.Name,
                Timestamp = state.Timestamp
            };
        }
    }
}
