using Gold.Redis.Common.Models.Commands;
using Gold.Redis.LowLevelClient.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Interfaces;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisCommandsExecutor : IRedisCommandExecutor
    {
        private readonly IRedisCommandHandler _redisCommandHandler;
        private readonly JsonSerializer _jsonSerializer;

        public RedisCommandsExecutor(
            IRedisCommandHandler redisConnection,
            JsonSerializerSettings serializerSettings = null)
        {
            _redisCommandHandler = redisConnection;
            _jsonSerializer = JsonSerializer.Create(serializerSettings);
        }
        public async Task<T> Execute<T>(Command command)
        {
            var commandStr = command.GetCommandString();
            if (string.IsNullOrEmpty(commandStr))
            {
                throw new InvalidOperationException("Connot execute null command");
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
