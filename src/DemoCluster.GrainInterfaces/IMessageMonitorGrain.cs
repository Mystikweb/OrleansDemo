using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IMessageMonitorGrain : IGrainWithGuidKey
    {
        Task<bool> UpdateMonitor(MonitorConfig config);
        Task StartConsumer();
        Task StopConsumer();
    }
}