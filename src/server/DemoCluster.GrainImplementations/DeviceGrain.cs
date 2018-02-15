using System;
using DemoCluster.GrainInterfaces;
using Orleans;

namespace DemoCluster.GrainImplementations
{
    public class DeviceGrain : Grain, IDeviceGrain
    {
    }
}
