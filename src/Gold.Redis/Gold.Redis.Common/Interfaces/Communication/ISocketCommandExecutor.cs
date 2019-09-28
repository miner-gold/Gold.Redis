using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Communication
{
    public interface ISocketCommandExecutor
    {
        Task<string> ExecuteCommand(Socket socket, string command);
    }
}
