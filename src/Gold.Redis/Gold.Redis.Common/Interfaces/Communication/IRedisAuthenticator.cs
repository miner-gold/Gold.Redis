using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Communication
{
    public interface IRedisAuthenticator
    {
        Task<bool> TryAuthenticate(Socket connectionSocket, string password);
    }
}
