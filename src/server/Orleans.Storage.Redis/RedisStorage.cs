using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.Storage.Redis
{
    public class RedisStorage : IGrainStorage
    {
        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            throw new System.NotImplementedException();
        }

        public Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            throw new System.NotImplementedException();
        }

        public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            throw new System.NotImplementedException();
        }
    }
}