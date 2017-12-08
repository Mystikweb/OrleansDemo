using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using OrleansDemo.Interfaces;
using OrleansDemo.Models.Transfer;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OrleansDemo.Clients.CommandLine
{
    class Program
    {
        private static bool stopped = false;
        private static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Console.CancelKeyPress += Console_CancelKeyPress;

            RunMainAsync(args).GetAwaiter().GetResult();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            stopped = true;
            Console.WriteLine("Stopping process");
        }

        private static async Task RunMainAsync(string[] args)
        {
            var config = await GetDeviceConfiguration();

            Console.WriteLine("Waiting for server to start");
            await Task.Delay(TimeSpan.FromSeconds(15));

            using (var clusterClient = await StartClientWithRetries())
            {
                IDeviceGrain deviceGrain = clusterClient.GetGrain<IDeviceGrain>(config.DeviceId);

                await deviceGrain.Initialize(config);

                await deviceGrain.StartAsync();
                Console.WriteLine($"Started {config.Name} successfully");

                Task.Run(async () =>
                {
                    var randomGenerator = new Random();
                    while (!stopped)
                    {
                        int randomValue = randomGenerator.Next(1, 10);

                        try
                        {
                            await deviceGrain.RecordValue(randomValue.ToString());

                            Console.WriteLine($"Wrote {randomValue} to client.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Problems writing {randomValue} to client. Error is {ex.Message}");
                        }
                        finally
                        {
                            await Task.Delay(TimeSpan.FromSeconds(5));
                        }
                    }

                    await deviceGrain.StopAsync();

                    await clusterClient.Close();
                }).GetAwaiter().GetResult();
            }
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    client = BuildClient();

                    await client.Connect();
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }

            return client;
        }

        private static async Task<DeviceConfiguration> GetDeviceConfiguration()
        {
            DeviceConfiguration result = null;

            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(configuration.GetValue<string>("API:BaseAddress"));
                http.DefaultRequestHeaders.Accept.Clear();
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var res = await http.GetAsync($"api/runtime/configuration/{configuration.GetValue<string>("DeviceName")}");

                if (res.IsSuccessStatusCode)
                {
                    string resContent = await res.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<DeviceConfiguration>(resContent);
                }

            }

            return result;
        }

        private static IClusterClient BuildClient()
        {
            ClientConfiguration configuration = ClientConfiguration.LoadFromFile("ClientConfiguration.xml");
            return new ClientBuilder()
                .UseConfiguration(configuration)
                .AddApplicationPartsFromBasePath()
                .ConfigureLogging(logger => logger.AddConsole())
                .Build();
        }
    }
}
