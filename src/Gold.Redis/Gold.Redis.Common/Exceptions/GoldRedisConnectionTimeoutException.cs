using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.Common.Exceptions
{
    public class GoldRedisConnectionTimeoutException : Exception
    {
        public GoldRedisConnectionTimeoutException(string host, int port,int maxRetryCount, TimeSpan maxConnectTimeout): 
            base($"Could not connect to redis server.\nHost = {host}, port = {port}, Number of connection attempts = {maxRetryCount}." +
                 $"\nConnection timed-out after {maxConnectTimeout}")
        {

        }
    }
}
