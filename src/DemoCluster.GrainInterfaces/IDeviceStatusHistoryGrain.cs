using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceStatusHistoryGrain : IGrainWithGuidKey
    {
        Task<DeviceStatusState> GetCurrentStatus();
        Task<List<DeviceStatusHistory>> GetStatusHistory();
        Task<bool> UpdateStatus(DeviceStatusCommand update);
    }
}