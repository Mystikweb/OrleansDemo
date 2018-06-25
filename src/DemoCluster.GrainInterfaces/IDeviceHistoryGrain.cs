using DemoCluster.GrainInterfaces.States;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceHistoryGrain : IGrainWithGuidKey
    {
        Task<DeviceState>
    }
}
