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
        Task<DeviceConfig> SaveDeviceAsync(DeviceConfig device);
        Task RemoveDeviceAsync(string deviceId);
        Task<List<SensorConfig>> GetSensorListAsync();
        Task<SensorConfig> GetSensorAsync(int sensorId);
        Task<SensorConfig> SaveSensorAsync(SensorConfig sensor);
        Task RemoveSensorAsync(int sensorId);
    }
}