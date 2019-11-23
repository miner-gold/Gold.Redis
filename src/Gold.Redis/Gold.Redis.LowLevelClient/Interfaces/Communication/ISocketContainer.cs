using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Interfaces.Communication
{
    public interface ISocketContainer : IDisposable
    {
        Task ConnectAsync(string host, int port);
        Task<int> SendAsync(ArraySegment<byte> bytes, SocketFlags flag);
        Socket Socket { get; }
    }
}
