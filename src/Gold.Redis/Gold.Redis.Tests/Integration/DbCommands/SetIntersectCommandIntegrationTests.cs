using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "SetIntersect")]
    public class SetIntersectCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [Test]
        public async Task SetIntersect_ShouldReturnElementThatAreInBothSetsBetweenTwoSets()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var secondSetKey = Guid.NewGuid().ToString();
            var firstSetItems = new[] { "1", "2", "3" };
            var secondSetItems = new[] { "1", "3", "4" };

            await _client.SetAddMultiple<string>(firstSetKey, firstSetItems);
            await _client.SetAddMultiple<string>(secondSetKey, secondSetItems);

            //Act
            var response = await _client.SetIntersect<string>(firstSetKey, secondSetKey);

            //Assert
            response.Count().Should().Be(2);
            response.Should().Contain(new[] { "1", "3" });
        }

        [Test]
        public async Task SetIntersect_ShouldReturnItemsThatAreInAllFourSets()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var secondSetKey = Guid.NewGuid().ToString();
            var thirdSetKey = Guid.NewGuid().ToString();
            var fourthSetKey = Guid.NewGuid().ToString();

            var firstSetItems = new[] { "1", "2", "3", "6", "9", "10" };
            var secondSetItems = new[] { "1", "3", "4" };
            var thirdSetItems = new[] { "1", "3", "4" };
            var fourthSetItems = new[] { "6", "7", "8", "2", "1" };

            await Task.WhenAll(
                _client.SetAddMultiple<string>(firstSetKey, firstSetItems),
                _client.SetAddMultiple<string>(secondSetKey, secondSetItems),
                _client.SetAddMultiple<string>(thirdSetKey, thirdSetItems),
                _client.SetAddMultiple<string>(fourthSetKey, fourthSetItems));

            //Act
            var response = await _client.SetIntersect<string>(firstSetKey, secondSetKey, thirdSetKey, fourthSetKey);

            //Assert
            response.Count().Should().Be(1);
            response.ElementAt(0).Should().Be("1");
        }

        [Test]
        public async Task SetIntersect_ShouldReturnAllElements_WhenCheckingOnItself()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var firstSetItems = new[] { "1", "2", "3" };
            await _client.SetAddMultiple<string>(firstSetKey, firstSetItems);

            //Act
            var response = await _client.SetIntersect<string>(firstSetKey, firstSetKey);

            //Assert
            response.Should().Contain(firstSetItems);
        }

        [Test]
        public async Task SetIntersect_ShouldReturnZeroElements_WhenExecutingIntersectionOnNoneExistingSet()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var firstSetItems = new[] { "1", "2", "3" };
            await _client.SetAddMultiple<string>(firstSetKey, firstSetItems);

            //Act
            var response = await _client.SetIntersect<string>(firstSetKey, Guid.NewGuid().ToString());

            //Assert
            response.Should().BeEmpty();
        }

        [Test]
        public async Task SetIntersectStore_ShouldStoreIntersectionValuesOnTheResultingKey()
        {
            //Arrange
            var resultingSetKey = Guid.NewGuid().ToString();
            var firstSetKey = Guid.NewGuid().ToString();
            var secondSetKey = Guid.NewGuid().ToString();
            var firstSetItems = new[] { "1", "2", "3" };
            var secondSetItems = new[] { "1", "3", "4" };

            await _client.SetAddMultiple<string>(firstSetKey, firstSetItems);
            await _client.SetAddMultiple<string>(secondSetKey, secondSetItems);

            //Act
            var response = await _client.SetIntersectStore(resultingSetKey, firstSetKey, secondSetKey);
            var resultingSetItems = await _client.GetSetMembers<string>(resultingSetKey);
            //Assert
            response.Should().Be(2);
            resultingSetItems.Should().Contain(new[] { "1", "3" });
        }
    }
}
