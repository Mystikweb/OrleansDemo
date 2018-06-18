using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface ISensorStatusHistoryGrain : IGrainWithIntegerKey
    {
        Task<bool> GetIsReceiving();
        Task<List<SensorStatusHistory>> GetStatusHistory();
        Task<bool> UpdateStatus(SensorStatusCommand update);
    }
}