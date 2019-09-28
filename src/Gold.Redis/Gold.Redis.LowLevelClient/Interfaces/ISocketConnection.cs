using System;
using System.Net.Sockets;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface ISocketContainer : IDisposable
    {
        Socket Socket { get; }
    }
}
