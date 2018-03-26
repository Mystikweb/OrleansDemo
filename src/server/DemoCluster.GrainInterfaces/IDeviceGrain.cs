using System;
using System.Threading.Tasks;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task<bool> GetIsRunning();
        Task Start();
        Task Stop();
    }
}