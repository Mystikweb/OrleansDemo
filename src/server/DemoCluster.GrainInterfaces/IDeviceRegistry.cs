using DemoCluster.GrainInterfaces.Patterns;
using Orleans;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceRegistry : IRegistryGrain<IDeviceGrain>
    {
        Task Initialize();
    }
}