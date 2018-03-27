using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DemoDevice
{
    class Program
    {
        private static DeviceHost host;

        static void Main(string[] args)
        {
            var appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var streamConfig = appConfig.GetSection(StreamConfiguration.SECTION_NAME).Get<StreamConfiguration>();
            host = new DeviceHost(streamConfig);
            RunAsync();
        }

        static async void RunAsync()
        {
            await host.StartAsync((message) => Console.WriteLine(message));
            Console.WriteLine("Press [Enter] to terminate...");
            Console.ReadLine();

            await host.StopAsync();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
