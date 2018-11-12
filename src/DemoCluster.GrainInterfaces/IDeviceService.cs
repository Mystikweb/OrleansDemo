using DemoCluster.Models;
using Orleans.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceService : IGrainService
    {
        Task<List<DeviceSummaryViewModel>> GetDevices();
        Task<DeviceSummaryViewModel> AddDevice(DeviceViewModel device);
        Task<DeviceSummaryViewModel> UpdateDevice(DeviceViewModel device);
        Task RemoveDevice(DeviceViewModel device);
    }
}
