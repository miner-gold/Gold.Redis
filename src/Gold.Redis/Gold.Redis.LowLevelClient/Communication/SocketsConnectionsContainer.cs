using Gold.Redis.Common.Interfaces.Communication;
using Gold.Redis.Common.Models.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class SocketsConnectionsContainer : IConnectionsContainer, IDisposable
    {
        private readonly RedisConnectionConfiguration _configuration;
        private readonly Random _random;
        private readonly ConcurrentDictionary<Socket, ManualResetEventSlim> _sockets;

        public SocketsConnectionsContainer(RedisConnectionConfiguration configuration)
        {
            _random = new Random();
            _configuration = configuration;

            _sockets = new ConcurrentDictionary<Socket, ManualResetEventSlim>();

            for (int i = 0; i < configuration.MaxConnections; i++)
            {
                _sockets.TryAdd(
                    new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp),
                    new ManualResetEventSlim(true));
            }
        }
        public async Task<Socket> GetSocket()
        {
            var openPair = GetFreeToUsePair();
            openPair.Value.Wait();

            if (openPair.Key.Connected)
            {
                return openPair.Key;
            }
            return await Connect(openPair.Key);
        }

        public void FreeSocket(Socket socket)
        {
            _sockets[socket].Set();
        }

        private KeyValuePair<Socket, ManualResetEventSlim> GetFreeToUsePair()
        {
            bool found = false;
            foreach (var pair in _sockets)
            {
                if (pair.Value.IsSet)
                {
                    return pair;
                }
            }

            //All of the sockets are is use, chose random socket and wait
            var randomValue = _random.Next(_configuration.MaxConnections);
            return _sockets.ToArray()[randomValue];
        }

        private async Task<Socket> Connect(Socket socket)
        {
            await socket.ConnectAsync(_configuration.Host, _configuration.Port);
            return socket;
        }

        public void Dispose()
        {
            foreach (var pair in _sockets)
            {
                pair.Key.Dispose();
                pair.Value.Dispose();
            }
        }

    }
}
