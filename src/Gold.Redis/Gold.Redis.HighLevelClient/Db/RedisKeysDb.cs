using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.HighLevelClient.Models.Commands.Keys;
using Gold.Redis.HighLevelClient.Models.Utils;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisKeysDb : IRedisKeysDb
    {
        private readonly IRedisCommandExecutor _commandExecutor;
        private readonly IStringResponseParser _stringResponseParser;
        public RedisKeysDb(IRedisCommandExecutor commandExecutor, IStringResponseParser stringResponseParser)
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

    }
}
