using System.Net.Sockets;
using Gold.Redis.LowLevelClient.Interfaces;

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
