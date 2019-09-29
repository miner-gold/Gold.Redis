using Gold.Redis.LowLevelClient.Communication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.Common;
using Gold.Redis.Common.Configuration;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Parsers;
using Gold.Redis.LowLevelClient.Parsers.PrefixParsers;
using Gold.Redis.LowLevelClient.Responses;
using Gold.Redis.Tests.Helpers;

namespace Gold.Redis.Tests.Integration
{
    [TestFixture]
    public class RedisCommandHandlerIntegrationsTests
    {
        private RedisCommandHandler _client;
        private RedisConnectionConfiguration _configuration;

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

            _configuration = RedisConfigurationLoader.GetConfiguration();
            var socketCommandExecutor = new SocketCommandExecutor(new RequestBuilder(), responseParser);
            var authenticator = new RedisSocketAuthenticator(socketCommandExecutor);
            var connectionContainer = new SocketsConnectionsContainer(_configuration, authenticator);
            _client = new RedisCommandHandler(connectionContainer, socketCommandExecutor);
        }

        [Test]
        public async Task ExecuteCommand_Ping_ShouldReturnPong()
        {
            //Arrange 
            var command = "PING";

            //Act
            var results = await _client.ExecuteCommand(command);

            //Assert
            results.Should().Be("PONG");
        }

        [Test]
        public async Task ExecuteCommand_SetKey_ShouldReturnOk()
        {
            //Arrange 
            var command = $"SET {Guid.NewGuid()} {Guid.NewGuid()}";

            //Act
            var results = await _client.ExecuteCommand(command);

            //Assert
            results.Should().Be("OK");
        }

        [Test]
        public async Task ExecuteCommand_SetKeyAndGetItBack_ShouldReturnCorrectValue()
        {
            //Arrange 
            var key = Guid.NewGuid();
            var value = Guid.NewGuid();
            var setCommand = $"SET {key} {value}";
            var getCommand = $"GET {key}";

            //Act 
            await _client.ExecuteCommand(setCommand);
            var result = await _client.ExecuteCommand(getCommand);

            //Assert
            result.Should().Be($"{value}");
        }

        [Test]
        public async Task ExecuteCommand_SetKeyAndValidateIsExisting_ShouldReturnOne()
        {
            //Arrange 
            var key = Guid.NewGuid();
            var value = Guid.NewGuid();
            var setCommand = $"SET {key} {value}";
            var existsCommand = $"EXISTS {key}";

            //Act 
            await _client.ExecuteCommand(setCommand);
            var result = await _client.ExecuteCommand(existsCommand);

            //Assert
            result.Should().Be($"1");
        }
        [Test]
        public async Task ExecuteCommand_ExistOnKeyThatDoesNotExits_ShouldReturnZero()
        {
            //Arrange 
            var randomKey = Guid.NewGuid();
            var existsCommand = $"EXISTS {randomKey}";

            //Act 
            var result = await _client.ExecuteCommand(existsCommand);

            //Assert
            result.Should().Be($"0");
        }

        [Test]
        public void ExecuteCommand_PingWhenBadPassword_ShouldThrowAuthenticationError()
        {
            //Arrange 
            var randomPassword = Guid.NewGuid().ToString();
            _configuration.Password = randomPassword;
            var command = "PING";

            //Act
            Assert.ThrowsAsync<AuthenticationException>(async() =>await _client.ExecuteCommand(command));
        }
    }
}
