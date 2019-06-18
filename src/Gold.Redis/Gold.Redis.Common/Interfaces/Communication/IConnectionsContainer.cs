using Gold.Redis.Common.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Communication
{
    public interface IConnectionsContainer : IDisposable
    {
        Task<Socket> GetSocket();

        void FreeSocket(Socket socket);
    }
}
