using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "SetRemove")]
    public class SetRemoveCommandIntegrationTest : RedisDataBaseClientIntegrationTestsBase
    {
        [TestCase("1", "2", "3")]
        [TestCase("1")]
        [TestCase(1, 2, 3)]
        [TestCase('1', '2', '3')]
        [Test]
        public async Task SetRemove_ShouldRemoveItemsOnSetsAndReturnTrue_WhenItemsOnSet(params object[] items)
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            await _client.SetAddMultiple(setKey, items);

            //Act
            var removed = await _client.SetRemove(setKey, items);
            var setElements = await _client.GetSetMembers<object>(setKey);

            //Assert
            removed.Should().BeTrue();
            setElements.Should().BeEmpty();
        }

        [Test]
        public async Task SetRemove_ShouldRemoveItemsOnSetsAndReturnFalse_WhenNotAllItemsOnSet()
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            var items = new[] { "1", "2", "3" };
            var itemsToRemove = items.Concat(new[] { "4", "5" }).ToArray();
            await _client.SetAddMultiple(setKey, items);

            //Act
            var removed = await _client.SetRemove(setKey, itemsToRemove);
            var setElements = await _client.GetSetMembers<object>(setKey);

            //Assert
            removed.Should().BeFalse();
            setElements.Should().BeEmpty();
        }

        [Test]
        public async Task SetRemove_ShouldReturnFalse_WhenSetDoesNotExist()
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            var items = new[] { "1", "2", "3" };

            //Act
            var removed = await _client.SetRemove(setKey, items);

            //Assert
            removed.Should().BeFalse();
        }

    }
}
