using OrleansDemo.Server;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace OrleansDemo.CommandLine
{
    class Program
    {
        static int Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            string clusterConfiguration = ClusterSetup.Instance.BuildClusterConfiguration();

            return RunMainAsync(clusterConfiguration).Result;
        }

        private static async Task<int> RunMainAsync(string clusterConfiguration)
        {
            try
            {
                var host = new ServerHost(clusterConfiguration);
                await host.StartAsync();

                Console.WriteLine("\r\nServer running press Enter to stop...\r\n");
                Console.ReadLine();

                await host.StopAsync();

                Console.WriteLine("\r\nServer has stopped successfully. Press any key to exit...\r\n");
                Console.ReadKey();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
                return 1;
            }
        }
    }
}
