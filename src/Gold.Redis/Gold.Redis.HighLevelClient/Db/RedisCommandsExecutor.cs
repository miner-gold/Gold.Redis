using Gold.Redis.Common.Interfaces.Communication;
using Gold.Redis.Common.Interfaces.Db;
using Gold.Redis.Common.Models.Commands;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisCommandsExecutor : IRedisCommandExecutor
    {
        private readonly IRedisConnection _redisConnection;
        private readonly JsonSerializerSettings _serializerSettings;
        public RedisCommandsExecutor(
            IRedisConnection redisConnection,
            JsonSerializerSettings serializerSettings = null)
        {
            _redisConnection = redisConnection;
            _serializerSettings = serializerSettings;
        }
        public async Task<T> Execute<T>(Command command)
        {
            var commandStr = command.GetCommandString();
            if (string.IsNullOrEmpty(commandStr))
            {
                throw new InvalidOperationException("Connot execute null command");
            }

            var responseStr = await _redisConnection.ExecuteCommand(commandStr);
            return _serializerSettings == null ?
                JsonConvert.DeserializeObject<T>(responseStr) :
                JsonConvert.DeserializeObject<T>(responseStr, _serializerSettings);
        }
    }
}
