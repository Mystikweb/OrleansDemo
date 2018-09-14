using DemoCluster.DAL.Models;
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
        Task AddDevice(DeviceConfig config);
        Task RemoveDevice(DeviceConfig config);
        Task StartDevice(string deviceId);
        Task StopDevice(string deviceId);
    }
}