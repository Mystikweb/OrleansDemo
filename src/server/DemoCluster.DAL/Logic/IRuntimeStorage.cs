using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;

namespace DemoCluster.DAL
{
    public interface IRuntimeStorage
    {
        Task<List<DeviceSummary>> GetDashboardSummary();
        Task<List<DeviceStateItem>> GetDeviceHistory(Guid deviceId);
        Task<bool> StoreDeviceHistory(DeviceStateItem historyItem);
    }
}