using FluentAssertions;
using Gold.Redis.Common;
using Gold.Redis.HighLevelClient.Db;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Parsers;
using Gold.Redis.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gold.Redis.Tests.Integration
{
    [TestFixture]
    public class RedisGeneralOperationsDbIntegrationTests
    {
        private RedisGeneralOperationsDb _client;

        [SetUp]
        public void SetUp()
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

            var lowLevelClient = new RedisCommandHandler(
                new SocketsConnectionsContainer(configuration, authenticator), socketCommandExecutor);
            var commandExecutor = new RedisCommandsExecutor(lowLevelClient);
            _client = new RedisGeneralOperationsDb(commandExecutor);
        }

        [Test]
        public async Task IsKeyExist_KeyDoesNotExist_ShouldReturnFalse()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            var result = await _client.IsKeyExists(randomKey);

            //Assert
            result.Should().BeFalse();
        }
    }
}
