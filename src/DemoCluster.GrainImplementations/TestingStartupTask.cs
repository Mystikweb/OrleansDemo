using DemoCluster.DAL;
using DemoCluster.GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    public class TestingStartup : IStartupTask
    {
        private readonly ILogger logger;
        private readonly IGrainFactory grainFactory;
        private readonly IConfigurationStorage configuration;
        private readonly IRuntimeStorage runtime;
        private IDeviceRegistry registry;

        public TestingStartup(ILogger<TestingStartup> logger, 
            IGrainFactory grainFactory,
            IConfigurationStorage configuration,
            IRuntimeStorage runtime)
        {
            this.logger = logger;
            this.grainFactory = grainFactory;
            this.configuration = configuration;
            this.runtime = runtime;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            logger.LogInformation("Waiting 30 seconds to start tests");
            await Task.Delay(30000);

            
        }
    }
}