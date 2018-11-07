using DemoCluster.Models;
using Orleans;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface ISensorGrain : IGrainWithIntegerKey
    {
        Task<bool> UpdateModel(DeviceSensorViewModel model);
        Task RecordValue(SensorValueViewModel value);
        Task<SensorSummaryViewModel> GetSummary();
    }
}