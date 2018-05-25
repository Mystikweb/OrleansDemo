using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceInstanceGrain : IGrainWithGuidKey
    {
        Task<DeviceInstanceState> UpdateStatus(DeviceStateConfig newStatus);
        Task<DeviceInstanceState> UpdateSensors(List<SensorConfig> sensorList);
    }
}