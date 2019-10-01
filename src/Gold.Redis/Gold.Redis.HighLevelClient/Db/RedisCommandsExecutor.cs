using Gold.Redis.LowLevelClient.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Commands;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Responses;
using System.Collections.Generic;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisCommandsExecutor : IRedisCommandExecutor
    {
        private readonly IRedisCommandHandler _redisCommandHandler;
        public RedisCommandsExecutor(
            IRedisCommandHandler redisConnection)
        {
            _redisCommandHandler = redisConnection;
        }
        public async Task<T> Execute<T>(Command command) where T: Response
        {
            var commandStr = command.GetCommandString();
            if (string.IsNullOrEmpty(commandStr))
            {
                throw new InvalidOperationException("Can not execute null command");
            }

            return await _redisCommandHandler.ExecuteCommand<T>(commandStr);
        }
    }
}
