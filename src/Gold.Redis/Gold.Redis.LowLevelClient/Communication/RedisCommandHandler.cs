using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class RedisCommandHandler : IRedisCommandHandler
    {
        private readonly IConnectionsContainer _connections;
        private readonly ISocketCommandExecutor _socketCommandExecutor;
        public RedisCommandHandler(IConnectionsContainer connections, ISocketCommandExecutor socketCommand)
        {
            _connections = connections;
            _socketCommandExecutor = socketCommand;
        }

        public async Task<T> ExecuteCommand<T>(string command)
            where T : Response
        {
            using (var socketContainer = await _connections.GetSocket())
            {
                return await _socketCommandExecutor.ExecuteCommand<T>(socketContainer.Socket, command);
            }
        }
    }
}
