using System;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task<bool> GetIsRunning();
        Task Start(DeviceConfig config);
        Task Stop();
    }
}