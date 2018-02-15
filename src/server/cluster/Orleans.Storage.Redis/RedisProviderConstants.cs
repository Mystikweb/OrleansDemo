using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.Storage.Redis
{
    internal class RedisProviderConstants
    {
        public const string REDIS_CONNECTION_STRING = "RedisConnectionString";
        public const string REDIS_DATABASE_NUMBER = "DatabaseNumber";
        public const string USE_JSON_FORMAT_PROPERTY = "UseJsonFormat";
    }

    internal enum RedisProviderLogCode
    {
        ReadingRedisData = 10,
        WritingRedisData = 20,
        ClearingRedisData = 30
    }
}
