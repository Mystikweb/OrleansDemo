using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL.Database.Runtime;
using DemoCluster.DAL.Models;

namespace DemoCluster.DAL
{
    public interface IRuntimeStorage
    {
        Task<List<DeviceInstanceHistory>> GetDeviceStateHistory(Guid deviceId, int days = 30);
        Task<bool> SaveDeviceState(DeviceInstanceHistory item);
    }
}