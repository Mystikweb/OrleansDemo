using DemoCluster.Models;
using Orleans.Services;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceService : IGrainService
    {
        Task<IDeviceGrain> AddDevice(DeviceViewModel device);
        Task<IDeviceGrain> UpdateDevice(DeviceViewModel device);
        Task RemoveDevice(DeviceViewModel device);
    }
}
