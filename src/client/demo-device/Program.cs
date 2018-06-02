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

            string endpointLocation = appConfig.GetValue<string>("Endpoint");
            StreamConfiguration streamConfig = appConfig.GetSection(StreamConfiguration.SECTION_NAME).Get<StreamConfiguration>();
            host = new DeviceHost(streamConfig, endpointLocation, (message) => Console.WriteLine(message));
            RunAsync();
        }

        static async void RunAsync()
        {
            while (!host.CanRun)
            {
                Console.WriteLine("Enter the device name: ");
                string deviceName = Console.ReadLine();

                await host.GetDeviceConfigAsync(deviceName);
            }

            if (host.CanRun)
            {
                ExecuteAsync();
            }
        }

        static async void ExecuteAsync()
        {
            await host.StartAsync();
            Console.WriteLine("Press [Enter] to terminate...");
            Console.ReadLine();

            await host.StopAsync();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
