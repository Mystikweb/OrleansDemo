using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using Orleans.EventSourcing;
using Orleans.EventSourcing.CustomStorage;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName = "CustomStorage")]
    public class DeviceInstanceGrain :
        JournaledGrain<DeviceInstanceState, DeviceCommand>,
        ICustomStorageInterface<DeviceInstanceState, DeviceCommand>,
        IDeviceInstanceGrain
    {
        public Task<DeviceInstanceState> UpdateSensors(List<SensorConfig> sensorList)
        {
            throw new System.NotImplementedException();
        }

        public Task<DeviceInstanceState> UpdateStatus(DeviceStateConfig newStatus)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ApplyUpdatesToStorage(IReadOnlyList<DeviceCommand> updates, int expectedversion)
        {
            throw new System.NotImplementedException();
        }

        public Task<KeyValuePair<int, DeviceInstanceState>> ReadStateFromStorage()
        {
            throw new System.NotImplementedException();
        }
    }
}