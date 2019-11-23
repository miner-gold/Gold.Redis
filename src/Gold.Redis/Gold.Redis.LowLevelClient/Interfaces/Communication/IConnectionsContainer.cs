using System;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Interfaces.Communication
{
    public interface IConnectionsContainer : IDisposable
    {
        Task<ISocketContainer> GetSocket();

        void FreeSocket(ISocketContainer socket);
    }
}
