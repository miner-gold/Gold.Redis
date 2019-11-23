using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gold.Redis.Common.Configuration;
using Gold.Redis.Common.Exceptions;
using Gold.Redis.Common.Utils;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Interfaces.Communication;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class RedisCommandHandler : IRedisCommandHandler
    {
        private readonly IConnectionsContainer _connections;
        private readonly ISocketCommandExecutor _socketCommandExecutor;

        private readonly string _redisHostname;
        private readonly TimeSpan _requestTimeoutSpan;
        public RedisCommandHandler(
            IConnectionsContainer connections,
            ISocketCommandExecutor socketCommand,
            RedisConnectionConfiguration configuration)
        {
            _connections = connections;
            _socketCommandExecutor = socketCommand;
            _requestTimeoutSpan = configuration.RequestTimeout;
            _redisHostname = configuration.Host;
        }

        public async Task<IEnumerable<T>> ExecuteCommands<T>(IEnumerable<string> commands)
            where T : Response
        {
            using (var socketContainer = await _connections.GetSocket())
            {
                try
                {
                    return await _socketCommandExecutor.ExecuteCommands<T>(socketContainer, commands.ToArray())
                        .TimeoutAfter(_requestTimeoutSpan);
                }
                catch (TimeoutException)
                {
                    throw new GoldRedisRequestTimeoutException(_redisHostname, _requestTimeoutSpan, commands.ToArray());
                }
            }
        }

        public async Task<T> ExecuteCommand<T>(string command) where T : Response
        {
            try
            {
                using (var socketContainer = await _connections.GetSocket())
                {
                    return await _socketCommandExecutor.ExecuteCommand<T>(
                            socketContainer,
                            command)
                        .TimeoutAfter(_requestTimeoutSpan);
                }
            }
            catch (TimeoutException)
            {
                throw new GoldRedisRequestTimeoutException(_redisHostname, _requestTimeoutSpan, command);
            }
        }
    }
}

