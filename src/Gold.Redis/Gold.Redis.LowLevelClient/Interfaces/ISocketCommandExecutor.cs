using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface ISocketCommandExecutor
    {
        Task<string> ExecuteCommand(Socket socket, string command);
    }
}
