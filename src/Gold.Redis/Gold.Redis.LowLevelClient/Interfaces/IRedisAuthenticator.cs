using System.Net.Sockets;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces.Communication;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface IRedisAuthenticator
    {
        Task<bool> TryAuthenticate(ISocketContainer connectionSocket, string password);
    }
}
