using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;

namespace DemoCluster.DAL
{
    public interface IRuntimeStorage
    {
        Task<List<DeviceStateItem>> GetDeviceStateHistory(Guid deviceId, int days = 14);
        Task<DeviceStateItem> SaveDeviceState(DeviceStateItem item);
    }
}