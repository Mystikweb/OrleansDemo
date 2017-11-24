using Newtonsoft.Json;
using OrleansDemo.Models.Transfer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OrleansDemo.Server.Services
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly HttpClient http;

        public ConfigurationManager()
        {
            http = new HttpClient();
            http.BaseAddress = new Uri("http://localhost:11635/");
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<DeviceConfiguration>> GetDeviceConfigurations()
        {
            List<DeviceConfiguration> results = new List<DeviceConfiguration>();

            var res = await http.GetAsync("api/runtime/configuration");

            if (res.IsSuccessStatusCode)
            {
                string resContent = await res.Content.ReadAsStringAsync();

                results = JsonConvert.DeserializeObject<List<DeviceConfiguration>>(resContent);
            }

            return results;
        }

        public void Dispose()
        {
            http.Dispose();
        }
    }
}
