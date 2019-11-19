using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "UnionSet")]
    public class UnionSetCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [Test]
        public async Task UnionSet_ShouldReturnedTheUnionSet()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var secondSetKey = Guid.NewGuid().ToString();

            var firstSetItems = new string[] { "1", "2", "3" };
            var secondSetItems = new string[] { "3", "4", "5", "6" };

            await _client.SetAddMultiple(firstSetKey, firstSetItems);
            await _client.SetAddMultiple(secondSetKey, secondSetItems);

            //Act
            var unionSets = await _client.UnionSets<string>(firstSetKey, secondSetKey);

            //Assert
            unionSets.Should().BeEquivalentTo("1", "2", "3", "4", "5", "6");
            unionSets.Count().Should().Be(6);
        }

        [Test]
        public async Task UnionSet_ShouldReturnedTheSentItems_WhenUnionOneSet()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString(); ;
            var firstSetItems = new string[] { "1", "2", "3" };

            await _client.SetAddMultiple(firstSetKey, firstSetItems);

            //Act
            var unionSets = await _client.UnionSets<string>(firstSetKey);

            //Assert
            unionSets.Should().BeEquivalentTo("1", "2", "3");
            unionSets.Count().Should().Be(3);
        }

        [Test]
        public async Task UnionSet_ShouldReturnedTheOriginSet_WhenUnionSetThatNotExists()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString(); ;
            var firstSetItems = new string[] { "1", "2", "3" };

            await _client.SetAddMultiple(firstSetKey, firstSetItems);

            //Act
            var unionSets = await _client.UnionSets<string>(firstSetKey, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            //Assert
            unionSets.Should().BeEquivalentTo("1", "2", "3");
            unionSets.Count().Should().Be(3);
        }

        [Test]
        public async Task UnionSet_ShouldReturnedEmptyList_WhenUnionWithNoExistingSets()
        {
            //Act
            var unionSets = await _client.UnionSets<string>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            //Assert
            unionSets.Should().BeEmpty();
        }

        [Test]
        public async Task UnionSetStore_ShouldSaveTheUnionedSetAndReturnNumberOfItems()
        {
            //Arrange
            var firstSetKey = Guid.NewGuid().ToString();
            var secondSetKey = Guid.NewGuid().ToString();
            var resultsSet = Guid.NewGuid().ToString();

            var firstSetItems = new string[] { "1", "2", "3" };
            var secondSetItems = new string[] { "3", "4", "5", "6" };

            await _client.SetAddMultiple(firstSetKey, firstSetItems);
            await _client.SetAddMultiple(secondSetKey, secondSetItems);

            //Act
            var unionStore = await _client.UnionSetsStore(resultsSet, firstSetKey, secondSetKey);
            var resultsSets = await _client.GetSetMembers<string>(resultsSet);
            //Assert
            unionStore.Should().Be(6);
            resultsSets.Should().BeEquivalentTo("1", "2", "3", "4", "5", "6");
            resultsSets.Count().Should().Be(6);

        }

    }
}
