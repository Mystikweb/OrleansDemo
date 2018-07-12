using System.Threading.Tasks;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface IMessageHandlerGrain : IGrainWithGuidKey
    {
        Task RouteMessage(string message);       
    }
}