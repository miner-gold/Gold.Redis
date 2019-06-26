using Gold.Redis.Common.Models.Configuration;
using Gold.Redis.LowLevelClient.Communication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.Common;
using Gold.Redis.LowLevelClient.Parsers;

namespace Gold.Redis.Tests.Integration
{
    [TestFixture]
    public class RedisLowLevelClientIntegrationsTests
    {
        private RedisLowLevelClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new RedisLowLevelClient(
                new SocketsConnectionsContainer(
                    new RedisConnectionConfiguration
                    {
                        Host = "localhost",
                        Port = 6379,
                        MaxConnections = 4
                    }), new RequestBuilder(), new ResponseParser());
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
    }
}
