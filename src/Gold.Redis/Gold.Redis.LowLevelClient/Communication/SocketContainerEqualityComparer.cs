using System;
using System.Collections.Generic;
using System.Text;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Interfaces.Communication;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class SocketContainerEqualityComparer : IEqualityComparer<ISocketContainer>
    {
        public bool Equals(ISocketContainer x, ISocketContainer y) => x.Socket == y.Socket;

        public int GetHashCode(ISocketContainer obj) => obj.Socket.GetHashCode();

    }
}
