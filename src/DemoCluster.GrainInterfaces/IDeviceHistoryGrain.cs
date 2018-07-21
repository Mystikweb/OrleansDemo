using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceHistoryGrain : IGrainWithGuidKey
    {
        Task<DeviceState> Initialize();
        Task<DeviceState> UpdateConfig(DeviceConfig config);
        Task<List<DeviceState>> GetDeviceHistory(DateTime startDateUtc, DateTime? endDateUtc = null);
        Task<DeviceState> StoreDeviceHistory();
    }
}
