using System;
using System.Threading.Tasks;
using DemoCluster.DAL.States;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task<bool> GetIsRunning();
        Task Start();
    }
}