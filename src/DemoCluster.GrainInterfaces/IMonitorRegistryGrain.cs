using System.Threading;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.Patterns;

namespace DemoCluster.GrainInterfaces
{
    public interface IMonitorRegistryGrain : IRegistryGrain<IMessageMonitorGrain>
    {
        Task Initialize(CancellationToken cancellation);
        Task ConfigureMonitor(MonitorConfig config);
    }
}