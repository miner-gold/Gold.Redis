using Gold.Redis.Common.Models.Configuration;
using Gold.Redis.LowLevelClient.Communication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gold.Redis.IntegrationsTests
{
    [TestFixture]
    public class RedisLowLevelClientInegrationsTests
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
                    }));
        }

        [Test]
        public async Task ExecuteCommand_SetKey_ShouldReturnTrue()
        {
            //Arrange 
            var command = "*3\r\n$3\r\nSET\r\n$1\r\na\r\n$2\r\n10\r\n";

            //Act
            var results = await _client.ExecuteCommand(command);

            //Assert
            Assert.AreEqual("+OK",results);
        }
    }
}
