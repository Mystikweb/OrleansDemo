using DemoCluster.Util.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;

namespace DemoCluster.Util
{
    public static class DemoClusterUtilExtensions
    {
        public static ISiloHostBuilder UseRabbitMessaging(this ISiloHostBuilder builder, RabbitMessagingOptions options)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IRabbitConnection>(new RabbitConnection(options.HostName, 
                    options.Port,
                    options.UserName,
                    options.Password,
                    options.VirtualHost,
                    options.Namespace,
                    options.Exchange));
            });
            return builder;
        }
    }
}