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
    }
}
