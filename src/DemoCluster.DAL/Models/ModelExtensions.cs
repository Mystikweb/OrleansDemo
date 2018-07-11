using DemoCluster.DAL.Database.Configuration;
using DemoCluster.DAL.Database.Runtime;
using System;
using System.Linq;

namespace DemoCluster.DAL.Models
{
    public static class ModelExtensions
    {
        public static DeviceStateItem ConfigToStateItem(this DeviceStateConfig state, Guid deviceId)
        {
            return new DeviceStateItem
            {
                DeviceId = deviceId,
                DeviceStatusId = state.DeviceStateId.Value,
                StatusId = state.StateId,
                Name = state.Name,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}