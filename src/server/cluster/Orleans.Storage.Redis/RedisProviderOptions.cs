using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.Storage.Redis
{
    public class RedisProviderOptions
    {
        public const string SECTION_NAME = "Orleans.Storage.Redis";
        public string RedisConnectionString { get; set; } = "localhost";
        public int DatabaseNumber { get; set; } = 1;
        public bool UseJsonFormat { get; set; } = true;
    }
}
