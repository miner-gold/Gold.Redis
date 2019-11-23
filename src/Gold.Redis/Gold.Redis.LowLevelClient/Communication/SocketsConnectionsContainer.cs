using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Gold.Redis.Common.Configuration;
using Gold.Redis.Common.Exceptions;
using Gold.Redis.Common.Utils;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Interfaces.Communication;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class SocketsConnectionsContainer : IConnectionsContainer
    {
        private readonly RedisConnectionConfiguration _configuration;
        private readonly ISocketConnector _connector;
        private readonly IRedisAuthenticator _authenticator;
        private readonly ConcurrentDictionary<ISocketContainer, ManualResetEventSlim> _sockets;

        public SocketsConnectionsContainer(
            RedisConnectionConfiguration configuration,
            IRedisAuthenticator authenticator,
            ISocketConnector connector)
        {
            _configuration = configuration;
            _authenticator = authenticator;
            _connector = connector;
            _sockets = new ConcurrentDictionary<ISocketContainer, ManualResetEventSlim>(
                new SocketContainerEqualityComparer());
        }
        public async Task<ISocketContainer> GetSocket()
        {
            var openPair = await GetFreeToUsePair();
            openPair.Value.Wait();
            openPair.Value.Reset();

            if (openPair.Key.Socket.Connected)
            {
                return new SocketContainer(openPair.Key.Socket, this);
            }
            return new SocketContainer(await Connect(openPair.Key), this);
        }

        public void FreeSocket(ISocketContainer socket)
        {
            _sockets[socket].Set();
        }

        private async Task<KeyValuePair<ISocketContainer, ManualResetEventSlim>> GetFreeToUsePair()
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

        private async Task<Socket> Connect(ISocketContainer socket)
        {
            await _connector.ConnectSocket(socket);

            if (!string.IsNullOrEmpty(_configuration.Password))
            {
                if (!await _authenticator.TryAuthenticate(socket, _configuration.Password))
                    throw new AuthenticationException("The redis sever did not approved the authentication request. " +
                        $"Host = {_configuration.Host}");
            }

            return socket.Socket;
        }

        private async Task<KeyValuePair<ISocketContainer, ManualResetEventSlim>> WaitUntilSocketIsFree()
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
                new SocketContainer(this),
                new ManualResetEventSlim(true));
        }

        public void Dispose()
        {
            foreach (var pair in _sockets)
            {
                pair.Key.Socket.Dispose();
                pair.Value.Dispose();
            }
        }

    }
}
