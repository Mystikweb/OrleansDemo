using DemoCluster.Models;
using DemoCluster.States;
using Orleans;
using System;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task<Guid> GetKey();
        Task<DeviceState> GetState();
        Task<DeviceViewModel> GetDeviceModel();
        Task<bool> UpdateDevice(DeviceViewModel model);
        Task<CurrentDeviceStateViewModel> GetCurrentStatus();
        Task<bool> UpdateDeviceState(DeviceStateViewModel state);
    }
}