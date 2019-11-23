using System;

namespace Gold.Redis.Common.Configuration
{
    public class RedisConnectionConfiguration
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 6379;
        public string Password { get; set; }
        public int MaxConnections { get; set; } = 1;

        //Connection and Retries info
        public int ConnectionRetries = 3;
        public TimeSpan ConnectionFailedWaitTime = TimeSpan.FromSeconds(3);
        public TimeSpan RequestTimeout = TimeSpan.Zero;
        public TimeSpan ConnectTimeout = TimeSpan.Zero;
        public bool UsePiplining { get; set; }
        public RedisPipelineConfiguration Pipeline { get; set; }
    }
}
