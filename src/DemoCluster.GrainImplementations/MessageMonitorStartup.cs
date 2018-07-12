using System.Threading;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace DemoCluster.GrainImplementations
{
    public class MessageMonitorStartup : IStartupTask
    {
        public Task Execute(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}