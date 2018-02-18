using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.Storage.Redis
{
    public class RedisProviderOptions
    {
        public const string SECTION_NAME = "Orleans.Storage.Redis";
        public string Hostname { get; set; } = "localhost";
        public int Port { get; set; } = 6379;
        public string Password { get; set; } = string.Empty;
        public int DatabaseNumber { get; set; } = 1;
        public bool UseJsonFormat { get; set; } = true;
    }
}
