using Gold.Redis.Common.Interfaces.Db;
using Gold.Redis.Common.Models.Commands.Keys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisGeneralOperationsDb : IRedisDbGeneralOperations
    {
        private readonly IRedisCommandExecutor _commandExecutor;
        public RedisGeneralOperationsDb(IRedisCommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }
        public async Task<T> Get<T>(string key)
        {
            var command = new GetCommand
            {
                Key = key
            };

            return await _commandExecutor.Execute<T>(command);
        }

        public Task<IEnumerable<string>> GetMatchingKeys(string pattern)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsKeyExists(string key)
        {
            var command = new KeyExistsCommand
            {
                Key = key
            };

            return await _commandExecutor.Execute<bool>(command);
        }

        public async Task<bool> SetKeyExpire(string key, TimeSpan span)
        {
            var command = new ExpireKeyCommand
            {
                Key = key,
                Ttl = span
            };
            return await _commandExecutor.Execute<bool>(command);
        }
    }
}
