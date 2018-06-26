using DemoCluster.GrainInterfaces.Patterns;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceRegistry : IRegistryGrain<IDeviceWorkerGrain>
    {
        Task Initialize();
        Task Teardown();
        Task<bool> GetLoadedDeviceState(string deviceId);
        Task StartDevice(string deviceId);
        Task StopDevice(string deviceId);
    }
}