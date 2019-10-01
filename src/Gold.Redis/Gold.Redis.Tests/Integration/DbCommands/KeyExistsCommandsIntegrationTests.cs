using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "KeyExists")]
    public class KeyExistsCommandsIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [SetUp]
        public async Task SetUp() => await TestsSetUp();

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

        [Test]
        public async Task IsKeyExist_KeyDoesExists_ShouldReturnTrue()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomKey);
            var result = await _client.IsKeyExists(randomKey);

            //Assert
            result.Should().BeTrue();
        }
    }
}
