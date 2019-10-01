using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "SetKeyExpire")]
    public class SetKeyExpireIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [SetUp]
        public async Task SetUp() => await TestsSetUp();

        [Test]
        public async Task SetKeyExpire_ShouldHaveTheKeyForFourSecondsAndThenLossIs()
        {
            //Arrange
            var expiration = TimeSpan.FromSeconds(4);
            var randomKey = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomKey);
            await _client.SetKeyExpire(randomKey, expiration);
            var firstCheck = await _client.IsKeyExists(randomKey);
            await Task.Delay(expiration);
            var secondCheck = await _client.IsKeyExists(randomKey);

            //Assert
            firstCheck.Should().BeTrue();
            secondCheck.Should().BeFalse();
        }

        [Test]
        public async Task SetKeyExpire_ShouldReturnFalse_WhenSettingOnNoneExistingKey()
        {
            //Arrange
            var expiration = TimeSpan.FromSeconds(4);
            var randomKey = Guid.NewGuid().ToString();

            //Act
            var result = await _client.SetKeyExpire(randomKey, expiration);

            //Assert
            result.Should().BeFalse();
        }
    }
}
