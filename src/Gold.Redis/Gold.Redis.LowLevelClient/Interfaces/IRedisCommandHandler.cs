using System.Collections.Generic;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface IRedisCommandHandler
    {
        Task<IEnumerable<T>> ExecuteCommands<T>(IEnumerable<string> commands)
            where T : Response;


        Task<T> ExecuteCommand<T>(string command)
            where T : Response;

    }

}
