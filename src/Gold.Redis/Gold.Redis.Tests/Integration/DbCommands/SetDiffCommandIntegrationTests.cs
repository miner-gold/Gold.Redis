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
    [TestFixture(Category = "SetDiff")]
    public class SetDiffCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [Test]
        public async Task SetDiff_ShouldReturnDiffrenceBetweenTwoSets()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var secondSetKey = Guid.NewGuid().ToString();
            var firstSetItems = new[] { "1", "2", "3" };
            var secondSetItems = new[] { "1", "3", "4" };

            await _client.SetAddMultiple<string>(firstSetKey, firstSetItems);
            await _client.SetAddMultiple<string>(secondSetKey, secondSetItems);

            //Act
            var response = await _client.SetDiff<string>(firstSetKey, secondSetKey);

            //Assert
            response.Count().Should().Be(1);
            response.ElementAt(0).Should().Be("2");
        }

        [Test]
        public async Task SetDiff_ShouldReturnDiffrenceBetweenThreeSets()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var secondSetKey = Guid.NewGuid().ToString();
            var thirdSetKey = Guid.NewGuid().ToString();
            var fourthSetKey = Guid.NewGuid().ToString();

            var firstSetItems = new[] { "1", "2", "3", "6", "9", "10" };
            var secondSetItems = new[] { "1", "3", "4" };
            var thirdSetItems = new[] { "1", "3", "4" };
            var fourthSetItems = new[] { "6", "7", "8", "2" };

            await Task.WhenAll(
                _client.SetAddMultiple<string>(firstSetKey, firstSetItems),
                _client.SetAddMultiple<string>(secondSetKey, secondSetItems),
                _client.SetAddMultiple<string>(thirdSetKey, thirdSetItems),
                _client.SetAddMultiple<string>(fourthSetKey, fourthSetItems));

            //Act
            var response = await _client.SetDiff<string>(firstSetKey, secondSetKey, thirdSetKey, fourthSetKey);

            //Assert
            response.Count().Should().Be(2);
            response.Should().Contain(new[] { "9", "10" });
        }

        [Test]
        public async Task SetDiff_ShouldReturnNoElements_WhenCheckSetOnItself()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var firstSetItems = new[] { "1", "2", "3" };
            await _client.SetAddMultiple<string>(firstSetKey, firstSetItems);

            //Act
            var response = await _client.SetDiff<string>(firstSetKey, firstSetKey);

            //Assert
            response.Should().BeEmpty();
        }

        [Test]
        public async Task SetDiff_ShouldReturnFirstSetElements_WhenCheckSetDiffOnSetThatDoesNotExists()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var firstSetItems = new[] { "1", "2", "3" };
            await _client.SetAddMultiple<string>(firstSetKey, firstSetItems);

            //Act
            var response = await _client.SetDiff<string>(firstSetKey, Guid.NewGuid().ToString());

            //Assert
            response.Should().BeEquivalentTo(firstSetItems);
        }

        [Test]
        public async Task SetDiff_ShouldReturnNoItems_WhenCheckDiffOnSetThatDoesNotExists()
        {
            //Arrange
            var secondSetKey = Guid.NewGuid().ToString();
            var secondSetItems = new[] { "1", "2", "3" };
            await _client.SetAddMultiple<string>(secondSetKey, secondSetItems);

            //Act
            var response = await _client.SetDiff<string>(Guid.NewGuid().ToString(), secondSetKey);

            //Assert
            response.Should().BeEmpty();
        }

        [Test]
        public async Task SetDiffStore_ShouldSaveTheDiffToTheKey()
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
            var response = await _client.SetDiffStore(resultingSetKey, firstSetKey, secondSetKey);
            var resultingSetItems = await _client.GetSetMembers<string>(resultingSetKey);
            //Assert
            response.Should().Be(1);
            resultingSetItems.ElementAt(0).Should().Be("2");
        }
    }
}
