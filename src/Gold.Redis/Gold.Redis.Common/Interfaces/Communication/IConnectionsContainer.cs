using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Communication
{
    public interface IConnectionsContainer : IDisposable
    {
        Task<ISocketContainer> GetSocket();

        void FreeSocket(Socket socket);
    }
}
