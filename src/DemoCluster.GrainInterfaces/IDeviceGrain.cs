using DemoCluster.Models;
using Orleans;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task<DeviceSummaryViewModel> GetDeviceSummary();
        Task<DeviceViewModel> GetDeviceModel();
        Task<bool> UpdateDevice(DeviceViewModel model, bool runDevice = true);
        Task<CurrentDeviceStateViewModel> GetCurrentStatus();
        Task<bool> UpdateDeviceState(DeviceStateViewModel state);
        Task<bool> Start(DeviceViewModel model = null);
        Task<bool> Stop();
    }
}