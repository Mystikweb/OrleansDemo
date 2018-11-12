using DemoCluster.Models;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceServiceGrain : IGrainWithIntegerKey
    {
        Task<List<DeviceSummaryViewModel>> GetDevices();
        Task<DeviceSummaryViewModel> AddDevice(DeviceViewModel device);
        Task<DeviceSummaryViewModel> UpdateDevice(DeviceViewModel device);
        Task RemoveDevice(DeviceViewModel device);
    }
}
