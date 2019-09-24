using Gold.Redis.Common.Interfaces.Communication;
using Gold.Redis.Common.Interfaces.Db;
using Gold.Redis.Common.Models.Commands;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisCommandsExecutor : IRedisCommandExecutor
    {
        private readonly IRedisCommandHandler _redisCommandHandler;
        private readonly JsonSerializer _jsonSerializer;

        public RedisCommandsExecutor(
            IRedisCommandHandler redisCommandHandler,
            JsonSerializerSettings serializerSettings = null)
        {
            _redisCommandHandler = redisCommandHandler;
            _jsonSerializer = JsonSerializer.Create(serializerSettings);
        }
        public async Task<T> Execute<T>(Command command)
        {
            var commandStr = command.GetCommandString();
            if (string.IsNullOrEmpty(commandStr))
            {
                throw new InvalidOperationException("Can not execute null command");
            }

            var responseStr = await _redisCommandHandler.ExecuteCommand(commandStr);
            using(var stringReader = new StringReader(responseStr))
            using(var jsonReader = new JsonTextReader(stringReader))
            {
                return _jsonSerializer.Deserialize<T>(jsonReader);
            }
        }
    }
}
