using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task<DeviceConfig> GetCurrentConfig();
        Task<bool> UpdateConfig();
        Task<DeviceStateItem> GetCurrentStatus();
        Task<bool> UpdateCurrentStatus(DeviceStateItem state);
    }
}