using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace DemoCluster.GrainInterfaces.Patterns
{
    public interface IRegistryGrain<TRegisteredGrain> : IGrainWithGuidKey
        where TRegisteredGrain : IGrain
    {
        Task<TRegisteredGrain> RegisterGrain(TRegisteredGrain item);

        Task<List<TRegisteredGrain>> GetRegisteredGrains();
    }
}