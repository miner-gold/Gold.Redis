using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.HighLevelClient.Commands.General;
using Gold.Redis.HighLevelClient.Commands.Keys;
using Gold.Redis.HighLevelClient.Commands.Search;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.HighLevelClient.Utils;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisGeneralOperationsDb : IRedisDbGeneralOperations
    {
        private readonly IRedisCommandExecutor _commandExecutor;
        private readonly IStringResponseParser _stringResponseParser;
        public RedisGeneralOperationsDb(IRedisCommandExecutor commandExecutor, IStringResponseParser stringResponseParser)
        {
            _commandExecutor = commandExecutor;
            _stringResponseParser = stringResponseParser;
        }
        /// <summary>
        /// Get the value of a key,
        /// If the key does not exists in the db, the default value of T will be returned
        /// </summary>
        /// <typeparam name="T">The expected Type of the returned value</typeparam>
        /// <param name="key">The redis key</param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key)
        {
            var command = new GetKeyValueCommand
            {
                Key = key
            };

            var response = await _commandExecutor.Execute<BulkStringResponse>(command);
            return _stringResponseParser.Parse<T>(response?.Response);
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

        public async Task<bool> SetKey<T>(string key, T value, TimeSpan? expirySpan = null, KeyAssertion assertion = KeyAssertion.Any)
        {
            var command = new SetKeyCommand
            {
                Key = key,
                Value = _stringResponseParser.Stringify(value),
                Assertion = assertion,
                ExpirySpan = expirySpan
            };
            var response = await _commandExecutor.Execute<SimpleStringResponse>(command);
            return response?.Response == Constants.OkResponse;
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
