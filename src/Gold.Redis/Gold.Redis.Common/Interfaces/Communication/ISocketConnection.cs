using System;
using System.Net.Sockets;

namespace Gold.Redis.Common.Interfaces.Communication
{
    public interface ISocketContainer : IDisposable
    {
        Socket Socket { get; }
    }
}
