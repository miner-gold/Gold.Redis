using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Gold.Redis.Common.Interfaces;
using Gold.Redis.Common.Interfaces.Communication;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class SocketContainer : ISocketContainer
    {
        public Socket Socket { get; }
        public IConnectionsContainer ConnectionsContainer { get; set; }

        public SocketContainer(Socket socket, IConnectionsContainer connectionsContainer)
        {
            Socket = socket;
            ConnectionsContainer = connectionsContainer;
        }

        public void Dispose()
        {
            ConnectionsContainer.FreeSocket(Socket);
        }
    }
}
