﻿using System.Threading.Tasks;
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

        public async Task<Response> ExecuteCommand(string command)
        {
            using (var socketContainer = await _connections.GetSocket())
            {
                return await _socketCommandExecutor.ExecuteCommand(socketContainer.Socket, command);
            }
        }

       
    }
}
