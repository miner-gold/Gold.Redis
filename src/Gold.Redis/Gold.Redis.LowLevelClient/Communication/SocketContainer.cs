using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading.Tasks;
using Gold.Redis.Common.Configuration;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Interfaces.Communication;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class SocketContainer : ISocketContainer
    {
        public async Task ConnectAsync(string host, int port)
        {
            await Socket.ConnectAsync(host, port);
        }

        public async Task<int> SendAsync(ArraySegment<byte> bytes, SocketFlags flag)
        {
            return await Socket.SendAsync(bytes, flag);
        }

        public Socket Socket { get; }
        public IConnectionsContainer ConnectionsContainer { get; set; }

        public SocketContainer(Socket socket, IConnectionsContainer connectionsContainer)
        {
            Socket = socket;
            ConnectionsContainer = connectionsContainer;
        }

        public SocketContainer(IConnectionsContainer connectionsContainer)
        {
            Socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            ConnectionsContainer = connectionsContainer;
        }
          
        public void Dispose()
        {
            ConnectionsContainer.FreeSocket(this);
        }

    }
}
