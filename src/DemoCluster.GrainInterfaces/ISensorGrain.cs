using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface ISensorGrain : IGrainWithIntegerKey
    {
        Task<bool> UpdateConfig(DeviceSensorConfig config);
        Task RecordValue(SensorValueItem value);
        Task<SensorStateSummary> GetDeviceState();
    }
}