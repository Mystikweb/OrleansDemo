using System.Threading.Tasks;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IMessageMonitorGrain : IGrainWithStringKey
    {
        Task StartConsumer();
        Task StopConsumer();
    }
}