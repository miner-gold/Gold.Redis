using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "GetMatchingKeys")]
    public class GetMatchingKeysIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [SetUp]
        public async Task SetUp() => await TestsSetUp();

        [Test]
        public async Task GetMatchingKeys_ShouldReturnAllKeys_WhenPatternIsAll()
        {
            //Arrange
            var pattern = "*";

            //Act
            for (int i = 1; i < 4; i++)
            {
                await _client.SetKey("KEY_" + i, i);
            }

            var keys = await _client.GetMatchingKeys(pattern);

            //Assert
            keys.Count().Should().Be(3);
        }

        [Test]
        public async Task GetMatchingKeys_ShouldReturnOnlyKeysThatEndsWithHello_WhenPatternIsEndWithHello()
        {
            //Arrange
            var pattern = "*Hello";

            //Act
            await _client.SetKey("Hello", "1");
            await _client.SetKey("BY", "0");
            await _client.SetKey("blablaHello", "2");
            await _client.SetKey("blablaHello123", "0");
            await _client.SetKey("123Hello", "3");

            var keys = await _client.GetMatchingKeys(pattern);

            //Assert
            keys.Count().Should().Be(3);
        }

        [Test]
        public async Task GetMatchingKeys_ShouldReturnEmptyList_WhenNoMatchingKeysExists()
        {
            //Arrange
            var pattern = "*Hello";

            //Act
            await _client.SetKey("BY", "0");
            await _client.SetKey("blablaHello123", "0");

            var keys = await _client.GetMatchingKeys(pattern);

            //Assert
            keys.Count().Should().Be(0);
        }

        [Test]
        public async Task GetMatchingKeys_ShouldReturnEmptyList_WhenNoKeysExistsInDb()
        {
            //Arrange
            var pattern = "*";

            //Act
            var keys = await _client.GetMatchingKeys(pattern);

            //Assert
            keys.Count().Should().Be(0);
        }
    }
}
