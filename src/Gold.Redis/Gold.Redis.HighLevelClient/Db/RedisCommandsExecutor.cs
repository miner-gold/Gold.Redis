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
        private readonly IRedisConnection _redisConnection;
        private readonly JsonSerializer _jsonSerializer;

        public RedisCommandsExecutor(
            IRedisConnection redisConnection,
            JsonSerializerSettings serializerSettings = null)
        {
            _redisConnection = redisConnection;
            _jsonSerializer = JsonSerializer.Create(serializerSettings);
        }
        public async Task<T> Execute<T>(Command command)
        {
            var commandStr = command.GetCommandString();
            if (string.IsNullOrEmpty(commandStr))
            {
                throw new InvalidOperationException("Connot execute null command");
            }

            var responseStr = await _redisConnection.ExecuteCommand(commandStr);
            using(var stringReader = new StringReader(responseStr))
            using(var jsonReader = new JsonTextReader(stringReader))
            {
                return _jsonSerializer.Deserialize<T>(jsonReader);
            }
        }
    }
}
