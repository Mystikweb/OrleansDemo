using DemoCluster.GrainInterfaces.Patterns;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceRegistry : IRegistryGrain<IDeviceGrain>
    {
        Task Initialize();
        Task Teardown();
        Task<List<DeviceState>> GetLoadedDeviceStates();
        Task StartDevice(string deviceId);
        Task StopDevice(string deviceId);
    }
}