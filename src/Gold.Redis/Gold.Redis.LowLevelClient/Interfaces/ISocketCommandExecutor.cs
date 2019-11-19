using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface ISocketCommandExecutor
    {
        Task<IEnumerable<T>> ExecuteCommands<T>(Socket socket, params string[] commands)
            where T : Response;

        Task<T> ExecuteCommand<T>(Socket socket, string command)
            where T : Response;
    }
}
