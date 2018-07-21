using System;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task<Guid> GetKey();
        Task<DeviceState> GetState();
        Task<DeviceConfig> GetCurrentConfig();
        Task<bool> UpdateConfig(DeviceConfig config);
        Task<DeviceStateItem> GetCurrentStatus();
        Task<bool> UpdateCurrentStatus(DeviceStateItem state);
    }
}