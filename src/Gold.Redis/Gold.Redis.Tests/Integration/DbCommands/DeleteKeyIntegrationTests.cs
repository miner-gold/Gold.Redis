using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "DeleteKey")]
    public class DeleteKeyIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [Test]
        public async Task DeleteKey_ShouldDeleteTheExistingKey()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomKey);
            var firstCheck = await _client.IsKeyExists(randomKey);
            await _client.DeleteKey(randomKey);
            var secondCheck = await _client.IsKeyExists(randomKey);

            //Assert
            firstCheck.Should().BeTrue();
            secondCheck.Should().BeFalse();
        }

        [Test]
        public async Task DeleteKey_ShouldReturnFalse_WhenTryingToDeleteNonExistingKey()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            var result = await _client.DeleteKey(randomKey);

            //Assert
            result.Should().BeFalse();
        }
    }
}
