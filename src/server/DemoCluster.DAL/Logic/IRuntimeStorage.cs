using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;

namespace DemoCluster.DAL
{
    public interface IRuntimeStorage
    {
        Task<List<DeviceSummary>> GetDashboardSummary();
        Task<List<DeviceStateItem>> GetDeviceStates();
        Task<DeviceStateItem> GetInitialState(Guid deviceId);
        Task<List<DeviceHistoryItem>> GetDeviceHistory(Guid deviceId);
        Task<int> StoreDeviceHistory(DeviceHistoryItem historyItem);
        Task StoreSensorValue(SensorValueItem sensorValue);
    }
}