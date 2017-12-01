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

            RunMainAsync(args).GetAwaiter().GetResult();
        }

        static async Task RunMainAsync(string[] args)
        {
            var config = await GetDeviceConfiguration();

            await Task.Delay(TimeSpan.FromSeconds(15));

            using (var clusterClient = await StartClientWithRetries())
            {
                IDeviceGrain deviceGrain = clusterClient.GetGrain<IDeviceGrain>(config.DeviceId);

                await deviceGrain.Initialize(config);

                await deviceGrain.Start();

                Task.Run(async () =>
                {
                    var randomGenerator = new Random();
                    while (!stopped)
                    {
                        int randomValue = randomGenerator.Next(1, 10);

                        await deviceGrain.RecordValue(randomValue.ToString());

                        await Task.Delay(TimeSpan.FromSeconds(5));
                    }
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
                    var config = ClientConfiguration.LocalhostSilo();
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
                .ConfigureLogging(logger =>
                {
                    if (Environment.UserInteractive)
                    {
                        logger.AddConsole();
                    }
                }).Build();
        }
    }
}
