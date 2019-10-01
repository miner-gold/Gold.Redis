using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "Get")]
    public class GetCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [SetUp]
        public async Task Setup() => await TestsSetUp();

        [Test]
        public async Task Get_KeyExists_ShouldReturnTheInsertedValue()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();
            var randomValue = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomValue);
            var result = await _client.Get<string>(randomKey);

            //Assert
            result.Should().Be(randomValue);
        }

        [Test]
        public async Task Get_KeyDoesNotExists_ShouldReturnNull()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            var result = await _client.Get<string>(randomKey);

            //Assert
            Assert.Null(result);
        }
    }
}
