using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using Orleans.EventSourcing;
using Orleans.EventSourcing.CustomStorage;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName="LogStorage")]
    public class SensorStatusHistoryGrain :
        JournaledGrain<SensorStatusState, SensorStatusCommand>,
        ICustomStorageInterface<SensorStatusState, SensorStatusCommand>,
        ISensorStatusHistoryGrain
    {
        

        public Task<bool> GetIsReceiving()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SensorStatusHistory>> GetStatusHistory()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateStatus(SensorStatusCommand update)
        {
            throw new System.NotImplementedException();
        }

        public Task<KeyValuePair<int, SensorStatusState>> ReadStateFromStorage()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ApplyUpdatesToStorage(IReadOnlyList<SensorStatusCommand> updates, int expectedversion)
        {
            throw new System.NotImplementedException();
        }
    }
}