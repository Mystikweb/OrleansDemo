using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;

namespace DemoCluster.DAL
{
    public interface IRuntimeStorage
    {
        Task<List<DeviceHistoryItem>> GetDeviceStateHistory(Guid deviceId, int days = 14);
        Task<DeviceHistoryItem> SaveDeviceState(DeviceHistoryItem item);
    }
}