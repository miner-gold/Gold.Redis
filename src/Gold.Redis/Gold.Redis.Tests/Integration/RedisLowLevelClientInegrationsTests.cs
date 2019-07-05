using Gold.Redis.Common.Models.Configuration;
using Gold.Redis.LowLevelClient.Communication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.LowLevelClient.Parsers;
using Gold.Redis.Tests.AssertExtensions;

namespace Gold.Redis.Tests.Integration
{
    [TestFixture]
    public class RedisLowLevelClientIntegrationsTests
    {
        private RedisLowLevelClient _client;

        [SetUp]
        public void SetUp()
        {
            var prefixParsers = new Dictionary<RedisResponseTypes, IPrefixParser>
            {
                {RedisResponseTypes.SimpleString, new SimpleStringParser()},
                {RedisResponseTypes.BulkString, new BulkStringParser()},
                {RedisResponseTypes.Integer, new IntegerParser()},
                {RedisResponseTypes.Error, new ErrorParser() }
            };
            var responseParser = new ResponseParser(prefixParsers
                .Concat(new[]
                    {new KeyValuePair<RedisResponseTypes, IPrefixParser>(RedisResponseTypes.Array, new ArrayParser(prefixParsers))})
                .ToDictionary(d => d.Key, d => d.Value));

            _client = new RedisLowLevelClient(
                new SocketsConnectionsContainer(
                    new RedisConnectionConfiguration
                    {
                        Host = "localhost",
                        Port = 6666,
                        MaxConnections = 4
                    }), new RequestBuilder(),
                responseParser);
        }

        [Test]
        public async Task ExecuteCommand_Ping_ShouldReturnPong()
        {
            //Arrange 
            var command = "PING";

            //Act
            var results = await _client.ExecuteCommand(command);

            //Assert
            results.Should().MessageBe("PONG", RedisResponse.SimpleString);
        }

        [Test]
        public async Task ExecuteCommand_SetKey_ShouldReturnOk()
        {
            //Arrange 
            var command = $"SET {Guid.NewGuid()} {Guid.NewGuid()}";

            //Act
            var results = await _client.ExecuteCommand(command);

            //Assert
            results.Should().MessageBe("OK", RedisResponse.SimpleString);
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
            result.Should().MessageBe(value.ToString(), RedisResponse.BulkString);
        }
    }
}
