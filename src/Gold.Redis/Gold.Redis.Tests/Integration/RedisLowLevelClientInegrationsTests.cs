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
    public class RedisLowLevelClientIntegrationsTests : RedisClientTest
    {
        private RedisLowLevelClient _client;

        [SetUp]
        public void SetUp()
        {
            var config = new RedisConnectionConfiguration
            {
                Host = "localhost",
                Port = 6379,
                MaxConnections = 4
            };
            _client = CreateClient(config);     
        }

        [Test]
        public async Task ExecuteCommand_Ping_ShouldReturnPong()
        {
            //Arrange 
            var command = "PING";

            //Act
            var results = await _client.ExecuteCommand(command);

            //Assert
            results.Should().MessageBe("PONG", RedisResponseTypes.SimpleString);
        }

        [Test]
        public async Task ExecuteCommand_SetKey_ShouldReturnOk()
        {
            //Arrange 
            var command = $"SET {Guid.NewGuid()} {Guid.NewGuid()}";

            //Act
            var results = await _client.ExecuteCommand(command);

            //Assert
            results.Should().MessageBe("OK", RedisResponseTypes.SimpleString);
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
            result.Should().MessageBe(value.ToString(), RedisResponseTypes.BulkString);
        }
    }
}
