using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.Common.Interfaces.Communication;
using Gold.Redis.Common.Interfaces.Parsers;
using Gold.Redis.Common.Models.Configuration;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Parsers;
using Gold.Redis.LowLevelClient.Parsers.MessageParsers;
using Gold.Redis.Tests.AssertExtensions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration
{
    [TestFixture]
    public class RedisNaiveClusterIntegrationTests : RedisClientTest
    {
        private RedisNaiveClusterClient _cluster;

        [SetUp]
        public void SetUp()
        {
            var host1Config = new RedisConnectionConfiguration
            {
                Host = "127.0.0.1",
                Port = 7000,
                MaxConnections = 4
            };
            var host2Config = new RedisConnectionConfiguration
            {
                Host = "127.0.0.1",
                Port = 7001,
                MaxConnections = 4
            };
            var clients = new List<KeyValuePair<RedisConnectionConfiguration, IRedisConnection>>()
            {
                new KeyValuePair<RedisConnectionConfiguration, IRedisConnection>(host1Config,
                    CreateClient(host1Config)),
                new KeyValuePair<RedisConnectionConfiguration, IRedisConnection>(host2Config, CreateClient(host2Config))
            };
            var parser = new MessageParser(new List<IMessageParser>() {new MovedParser()});
            _cluster = new RedisNaiveClusterClient(parser, clients);
        }
        [Test]
        public async Task ExecuteCommand_SetKeyHashInSecondHost_ShouldReturnOk()
        {
            // ab key hash slot is 13567 (AKA second host hash slots)
            //Arrange 
            var command = $"SET ab {Guid.NewGuid()}";

            //Act
            var results = await _cluster.ExecuteCommand(command);

            //Assert
            results.Should().MessageBe("OK", RedisResponseTypes.SimpleString);
        }

        [Test]
        public async Task ExecuteCommand__SetKeyHashInSecondHostAndGetItBack_ShouldReturnCorrectValue()
        {
            //Arrange 
            var value = Guid.NewGuid();
            var setCommand = $"SET ab {value}";
            var getCommand = $"GET ab";

            //Act
            await _cluster.ExecuteCommand(setCommand);
            var result = await _cluster.ExecuteCommand(getCommand);

            //Assert
            result.Should().MessageBe(value.ToString(), RedisResponseTypes.BulkString);
        }





    }
}
