using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.Common.Models.Configuration
{
    public class RedisConnectionConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 6379;
        public int MaxConnections { get; set; }
    }
}
