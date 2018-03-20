using DemoCluster.GrainInterfaces.Patterns;
using Orleans;
using System;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceRegistry : IRegistryGrain<IDeviceGrain>
    {
        Task Initialize();
        Task StartDevice(string deviceId);
    }
}