using System;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task<DeviceState> GetCurrentState();
        Task Start();
        Task Stop();
    }
}