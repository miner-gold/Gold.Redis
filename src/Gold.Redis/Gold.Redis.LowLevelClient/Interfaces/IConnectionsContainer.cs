using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface IConnectionsContainer : IDisposable
    {
        Task<ISocketContainer> GetSocket();

        void FreeSocket(Socket socket);
    }
}
