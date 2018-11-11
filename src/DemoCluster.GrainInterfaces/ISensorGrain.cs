using DemoCluster.Models;
using Orleans;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface ISensorGrain : IGrainWithIntegerKey
    {
        Task<SensorSummaryViewModel> GetSummary();
        Task<bool> UpdateModel(DeviceSensorViewModel model, bool runSensor = true);
        Task RecordValue(SensorValueViewModel value);
        Task<bool> Start(bool withUpdate = true);
        Task<bool> Stop();
    }
}