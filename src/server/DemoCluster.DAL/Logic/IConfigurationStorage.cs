using DemoCluster.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.DAL
{
    public interface IConfigurationStorage
    {
        Task<List<DeviceConfig>> GetDeviceListAsync();
        Task<DeviceConfig> GetDeviceByIdAsync(string deviceId);
        Task<DeviceConfig> GetDeviceByNameAsync(string name);
        Task<DeviceConfig> SaveDeviceAsync(DeviceConfig model);
        Task RemoveDeviceAsync(string deviceId);
        Task<List<SensorConfig>> GetSensorListAsync(string sort, int index, string search);
        Task<SensorConfig> GetSensorAsync(int sensorId);
        Task<SensorConfig> SaveSensorAsync(SensorConfig model);
        Task RemoveSensorAsync(int sensorId);
        Task<List<EventConfig>> GetEventListAsync(string sort, int index, string search);
        Task<EventConfig> GetEventAsync(int eventId);
        Task<EventConfig> SaveEventAsync(EventConfig model);
        Task RemoveEventAsync(int eventId);
    }
}