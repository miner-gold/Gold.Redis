using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "MoveItemBetweenSets")]
    public class MoveItemBetweenSetsCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [Test]
        public async Task MoveItemBetweenSets_ShouldMoveItemBetweenTheSets()
        {
            //Arrange
            var originSet = Guid.NewGuid().ToString();
            var destinationSet = Guid.NewGuid().ToString();
            var itemToMove = Guid.NewGuid().ToString();
            await _client.SetAdd(originSet, itemToMove);

            //Act
            var move = await _client.MoveItemBetweenSets(originSet, destinationSet, itemToMove);
            var itemExistOnOrigin = await _client.SetIsExists(originSet, itemToMove);
            var itemExistOnDestination = await _client.SetIsExists(destinationSet, itemToMove);

            //Assert
            move.Should().BeTrue();
            itemExistOnDestination.Should().BeTrue();
            itemExistOnOrigin.Should().BeFalse();

        }

        [Test]
        public async Task MoveItemBetweenSets_ShouldNotMoveItemBetweenTheSets_WhenItemNotOnOrigin()
        {
            //Arrange
            var originSet = Guid.NewGuid().ToString();
            var destinationSet = Guid.NewGuid().ToString();
            var randomItem = Guid.NewGuid().ToString();
            await _client.SetAdd(originSet, randomItem);

            //Act
            var move = await _client.MoveItemBetweenSets(originSet, destinationSet, Guid.NewGuid().ToString());

            //Assert
            move.Should().BeFalse();
        }
    }
}
