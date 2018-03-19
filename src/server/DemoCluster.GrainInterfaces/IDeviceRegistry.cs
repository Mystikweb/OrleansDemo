using DemoCluster.GrainInterfaces.Patterns;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceRegistry : IRegistryGrain<IDeviceGrain>
    {
    }
}