using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.HighLevelClient.Models.Commands.General;
using Gold.Redis.HighLevelClient.Models.Commands.Keys;
using Gold.Redis.HighLevelClient.Models.Commands.Search;
using Gold.Redis.HighLevelClient.Models.Utils;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisGeneralOperationsDb : IRedisGeneralOperationsDb
    {
        private readonly IRedisCommandExecutor _commandExecutor;
        public RedisGeneralOperationsDb(IRedisCommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        public async Task<IEnumerable<string>> GetMatchingKeys(string pattern = "*")
        {
            var command = new KeysCommand
            {
                Pattern = pattern
            };
            var response = await _commandExecutor.Execute<ArrayResponse>(command);

            return response?.Responses
                .OfType<BulkStringResponse>()
                .Select(item => item.Response);
        }

        public async Task<bool> DeleteKey(string key)
        {
            var command = new DeleteKeyCommand
            {
                Key = key
            };
            var response = await _commandExecutor.Execute<IntResponse>(command);

            return response.Response == 1;
        }

        public async Task<bool> FlushDb(bool isAsync = false)
        {
            var command = new FlushDbCommand
            {
                IsAsync = isAsync
            };
            var response = await _commandExecutor.Execute<SimpleStringResponse>(command);
            return response?.Response == Constants.OkResponse;
        }

        public async Task<bool> Ping()
        {
            var command = new PingCommand();
            var response = await _commandExecutor.Execute<SimpleStringResponse>(command);
            return response?.Response == "PONG";
        }

        public async Task<bool> IsKeyExists(string key)
        {
            var command = new KeyExistsCommand
            {
                Key = key
            };

            var response = await _commandExecutor.Execute<IntResponse>(command);
            return response?.Response == 1;
        }

        public async Task<bool> SetKeyExpire(string key, TimeSpan span)
        {
            var command = new ExpireKeyCommand
            {
                Key = key,
                Ttl = span
            };
            var response = await _commandExecutor.Execute<IntResponse>(command);
            return response?.Response == 1;
        }
    }
}
