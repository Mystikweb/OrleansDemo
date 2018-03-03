using DemoCluster.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.DAL
{
    public interface IConfigurationStorage
    {
        Task<List<DeviceConfig>> GetDeviceListAsync();
        Task<DeviceConfig> GetDeviceAsync(string deviceId);
        Task<DeviceConfig> SaveDeviceAsync(DeviceConfig device);
        Task<List<SensorConfig>> GetSensorListAsync();
        Task<SensorConfig> GetSensorAsync(int sensorId);
        Task<SensorConfig> SaveSensorAsync(SensorConfig sensor);
    }
}