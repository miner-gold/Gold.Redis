using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Gold.Redis.Common.Configuration;
using Gold.Redis.LowLevelClient.Interfaces;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class SocketsConnectionsContainer : IConnectionsContainer
    {
        private readonly RedisConnectionConfiguration _configuration;
        private readonly IRedisAuthenticator _authenticator;
        private readonly ConcurrentDictionary<Socket, ManualResetEventSlim> _sockets;

        public SocketsConnectionsContainer(
            RedisConnectionConfiguration configuration,
            IRedisAuthenticator authenticator)
        {
            _configuration = configuration;
            _authenticator = authenticator;
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
            if (!string.IsNullOrEmpty(_configuration.Password))
            {
                if (!await _authenticator.TryAuthenticate(socket, _configuration.Password))
                    throw new AuthenticationException("The redis sever did not approved the authentication request. " +
                        $"Host = {_configuration.Host}");
            }

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
