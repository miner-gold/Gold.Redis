using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "SetIsExist")]
    public class SetIsExistsIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [TestCase("1")]
        [TestCase(1)]
        [TestCase('1')]
        [TestCase(true)]
        [Test]
        public async Task SetIsExists__ShouldReturnTrue_WhenItemExistsOnSet(object item)
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            await _client.SetAdd(setKey, item);

            //Act
            var result = await _client.SetIsExists(setKey, item);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task SetIsExists_ShouldReturnFalse_WhenItemIsNotOnExistingSet()
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            var randomValue = Guid.NewGuid().ToString();
            await _client.SetAdd(setKey, randomValue);

            //Act
            var result = await _client.SetIsExists(setKey, Guid.NewGuid().ToString());

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task SetIsExists_ShouldReturnFalse_WhenSearchingOnNoneExistsingSet()
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            var randomValue = Guid.NewGuid().ToString();

            //Act
            var result = await _client.SetIsExists(setKey, randomValue);

            //Assert
            result.Should().BeFalse();
        }

    }
}
