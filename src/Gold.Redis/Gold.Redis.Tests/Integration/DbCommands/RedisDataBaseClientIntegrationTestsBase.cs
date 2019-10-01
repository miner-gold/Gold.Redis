using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.HighLevelClient.Db;
using Gold.Redis.HighLevelClient.ResponseParsers;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Parsers;
using Gold.Redis.LowLevelClient.Parsers.PrefixParsers;
using Gold.Redis.Tests.Helpers;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    public abstract class RedisDataBaseClientIntegrationTestsBase
    {
        protected RedisGeneralOperationsDb _client;

        public async Task TestsSetUp()
        {
            var prefixParsers = new Dictionary<char, IPrefixParser>
            {
                {CommandPrefixes.SimpleString, new SimpleStringParser()},
                {CommandPrefixes.BulkString, new BulkStringParser()},
                {CommandPrefixes.Integer, new IntegerParser()},
                {CommandPrefixes.Error, new ErrorParser() }
            };
            var responseParser = new ResponseParser(prefixParsers
                .Concat(new[]
                    {new KeyValuePair<char, IPrefixParser>(CommandPrefixes.Array, new ArrayParser(prefixParsers))})
                .ToDictionary(d => d.Key, d => d.Value));
            var configuration = RedisConfigurationLoader.GetConfiguration();
            var socketCommandExecutor = new SocketCommandExecutor(new RequestBuilder(), responseParser);
            var authenticator = new RedisSocketAuthenticator(socketCommandExecutor);
            var connectionContainer = new SocketsConnectionsContainer(configuration, authenticator);
            var lowLevelClient = new RedisCommandHandler(connectionContainer, socketCommandExecutor);


            var commandExecutor = new RedisCommandsExecutor(lowLevelClient);
            _client = new RedisGeneralOperationsDb(commandExecutor, new JsonResponseParser());
            await _client.FlushDb();
        }
    }
}
