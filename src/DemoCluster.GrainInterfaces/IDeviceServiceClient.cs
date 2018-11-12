using Orleans.Services;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceServiceClient : 
        IGrainServiceClient<IDeviceService>, 
        IDeviceService
    {
    }
}
