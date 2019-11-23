using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common.Configuration;
using Gold.Redis.Common.Exceptions;
using Gold.Redis.Common.Utils;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Interfaces.Communication;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class SocketConnectorWithRetries : ISocketConnector
    {
        private readonly RedisConnectionConfiguration _configuration;

        public SocketConnectorWithRetries(RedisConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task ConnectSocket(ISocketContainer socket)
        {
            bool isConnected = false;
            int retryCount = 0;
            while (!isConnected && retryCount < _configuration.ConnectionRetries)
            {
                try
                {
                    await socket.ConnectAsync(_configuration.Host, _configuration.Port)
                        .TimeoutAfter(_configuration.ConnectTimeout);
                    isConnected = true;
                }
                catch (TimeoutException)
                {
                    await Task.Delay(_configuration.ConnectionFailedWaitTime);
                    retryCount++;
                }
            }

            if (retryCount == _configuration.ConnectionRetries)
                throw new GoldRedisConnectionTimeoutException(
                    _configuration.Host,
                    _configuration.Port,
                    _configuration.ConnectionRetries,
                    _configuration.ConnectTimeout);
        }
    }
}
