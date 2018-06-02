using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.GrainInterfaces.States;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceStatusHistory
    {
        Task<DeviceStatusState> GetCurrentStatus();
        Task<List<DeviceStatusState>> GetStatusHistory();
        Task<bool> UpdateStatus(DeviceStatusState update);
    }
}