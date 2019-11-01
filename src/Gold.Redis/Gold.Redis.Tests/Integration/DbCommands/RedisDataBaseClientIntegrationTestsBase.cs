using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.HighLevelClient.Db;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.HighLevelClient.ResponseParsers;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Parsers;
using Gold.Redis.LowLevelClient.Parsers.PrefixParsers;
using Gold.Redis.Tests.Helpers;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    public abstract class RedisDataBaseClientIntegrationTestsBase
    {
        protected IRedisDb _client;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var prefixParsers = new Dictionary<char, IPrefixParser>
            {
                {CommandPrefixes.SimpleString, new SimpleStringParser()},
                {CommandPrefixes.BulkString, new BulkStringParser()},
                {CommandPrefixes.Integer, new IntegerParser()},
                {CommandPrefixes.Error, new ErrorParser() }
            };
            var parsers = new ResponseParser(prefixParsers
                .Concat(new[]
                    {new KeyValuePair<char, IPrefixParser>(CommandPrefixes.Array, new ArrayParser(prefixParsers))})
                .ToDictionary(d => d.Key, d => d.Value));
            var responseParser = new JsonResponseParser();

            var configuration = RedisConfigurationLoader.GetConfiguration();
            var socketCommandExecutor = new SocketCommandExecutor(new RequestBuilder(), parsers);
            var authenticator = new RedisSocketAuthenticator(socketCommandExecutor);
            var connectionContainer = new SocketsConnectionsContainer(configuration, authenticator);
            var lowLevelClient = new RedisCommandHandler(connectionContainer, socketCommandExecutor);

            var commandExecutor = new RedisCommandsExecutor(lowLevelClient);
            var commandExecutorHelper = new RedisCommandExecutorHelper(commandExecutor, responseParser);
            var redisScannerHelper = new RedisScanner(commandExecutor);

            var generalDb = new RedisGeneralOperationsDb(commandExecutor);
            var keyDb = new RedisKeysDb(commandExecutor, responseParser);
            var setDb = new RedisSetDb(commandExecutorHelper, responseParser, redisScannerHelper);

            _client = new RedisDb(generalDb, keyDb, setDb);
        }

        [SetUp]
        public async Task TestsSetUp()
        {
            await _client.FlushDb();
        }
    }
}
