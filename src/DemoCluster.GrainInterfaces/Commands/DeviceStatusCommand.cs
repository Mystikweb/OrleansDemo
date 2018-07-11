using System;
using DemoCluster.GrainInterfaces.States;

namespace DemoCluster.GrainInterfaces.Commands
{
    public class DeviceStatusCommand : DeviceCommand
    {
        public DeviceStatusCommand(int deviceStatusId, int statusId, string name, DateTime? timeStamp = null)
            : base(timeStamp)
        {
            DeviceStateId = deviceStatusId;
            StatusId = statusId;
            Name = name;
        }

        public int DeviceStateId { get; private set; }
        public int StatusId { get; private set; }
        public string Name { get; private set; }
    }
}