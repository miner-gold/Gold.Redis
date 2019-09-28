using System.Net.Sockets;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface ISocketCommandExecutor
    {
        Task<Response> ExecuteCommand(Socket socket, string command);
    }
}
