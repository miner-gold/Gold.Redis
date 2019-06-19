using Gold.Redis.Common.Models.Configuration;
using Gold.Redis.LowLevelClient.Communication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.LowLevelClient.Parsers;

namespace Gold.Redis.IntegrationsTests
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
        public async Task ExecuteCommand_SetKey_ShouldReturnTrue()
        {
            //Arrange 
            var command = $"SET {Guid.NewGuid()} {Guid.NewGuid()}";

            //Act
            var results = await _client.ExecuteCommand(command);

            //Assert
            results.Should().Be("+OK");
        }
    }
}
