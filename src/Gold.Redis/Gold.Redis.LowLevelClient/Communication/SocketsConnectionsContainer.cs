using Gold.Redis.Common.Interfaces.Communication;
using Gold.Redis.Common.Models.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class SocketsConnectionsContainer : IConnectionsContainer
    {
        private readonly RedisConnectionConfiguration _configuration;
        private readonly ConcurrentDictionary<Socket, ManualResetEventSlim> _sockets;

        public SocketsConnectionsContainer(RedisConnectionConfiguration configuration)
        {
            _configuration = configuration;

            _sockets = new ConcurrentDictionary<Socket, ManualResetEventSlim>();
        }
        public async Task<ISocketContainer> GetSocket()
        {
            var openPair = await GetFreeToUsePair();
            openPair.Value.Wait();
            openPair.Value.Reset();

            if (openPair.Key.Connected)
            {
                return new SocketContainer(openPair.Key, this);
            }
            return new SocketContainer(await Connect(openPair.Key), this);
        }

        public void FreeSocket(Socket socket)
        {
            _sockets[socket].Set();
        }

        private async Task<KeyValuePair<Socket, ManualResetEventSlim>> GetFreeToUsePair()
        {
            if (_sockets.Count < _configuration.MaxConnections)
            {
                AddSocket();
            }

            foreach (var pair in _sockets)
            {
                if (pair.Value.IsSet)
                {
                    return pair;
                }
            }
            
            return await WaitUntilSocketIsFree();
        }

        private async Task<Socket> Connect(Socket socket)
        {
            await socket.ConnectAsync(_configuration.Host, _configuration.Port);
            return socket;
        }

        private async Task<KeyValuePair<Socket, ManualResetEventSlim>> WaitUntilSocketIsFree()
        {
            var cancellationToken = new CancellationTokenSource();
            var freeSocket = await Task.WhenAny(_sockets.Select(socket => Task.Run(() =>
            {
                socket.Value.Wait(cancellationToken.Token);
                return socket;
            }, cancellationToken.Token)));
            cancellationToken.Cancel();
            return await freeSocket;
        }

        private void AddSocket()
        {
            _sockets.TryAdd(
                new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp),
                new ManualResetEventSlim(true));
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
