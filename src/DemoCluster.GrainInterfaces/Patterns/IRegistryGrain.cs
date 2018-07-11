using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace DemoCluster.GrainInterfaces.Patterns
{
    public interface IRegistryGrain<TRegisteredGrain> : IGrainWithIntegerKey
        where TRegisteredGrain : IGrain
    {
        Task<TRegisteredGrain> RegisterGrain(TRegisteredGrain item);
        Task<TRegisteredGrain> UnregisterGrain(TRegisteredGrain item);

        Task<List<TRegisteredGrain>> GetRegisteredGrains();
    }
}