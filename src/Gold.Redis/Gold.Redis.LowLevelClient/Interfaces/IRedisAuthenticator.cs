using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface IRedisAuthenticator
    {
        Task<bool> TryAuthenticate(Socket connectionSocket, string password);
    }
}
