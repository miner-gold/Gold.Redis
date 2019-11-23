using System.Collections.Generic;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Interfaces.Communication
{
    public interface ISocketCommandExecutor
    {
        Task<IEnumerable<T>> ExecuteCommands<T>(ISocketContainer socket, params string[] commands)
            where T : Response;

        Task<T> ExecuteCommand<T>(ISocketContainer socket, string command)
            where T : Response;
    }
}
