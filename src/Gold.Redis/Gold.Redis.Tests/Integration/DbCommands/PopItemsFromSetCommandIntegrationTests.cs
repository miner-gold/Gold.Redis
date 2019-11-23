using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "PopItemsFromSet")]
    public class PopItemsFromSetCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [Test]
        public async Task PopItemFromSet_ShouldRemoveOneItemFromTheSet()
        {
            //Arrange
            var set = Guid.NewGuid().ToString();
            var setItems = Enumerable.Range(0, 100).Select(i => Guid.NewGuid().ToString()).ToList();
            await _client.SetAddMultiple<string>(set, setItems);

            //Act
            var pop = await _client.PopItemFromSet<string>(set);
            var leftItems = await _client.GetSetMembers<string>(set);

            //Assert
            setItems.Should().Contain(pop);
            leftItems.Should().NotContain(pop);
        }

        [TestCase(10)]
        [TestCase(7)]
        [TestCase(150)]
        [Test]
        public async Task PopItemsFromSet_ShouldRemoveMultipleItemsFromTheSet(int numberOfItems)
        {
            //Arrange
            var set = Guid.NewGuid().ToString();
            var setItems = Enumerable.Range(0, 100).Select(i => Guid.NewGuid().ToString()).ToList();
            await _client.SetAddMultiple<string>(set, setItems);

            //Act
            var removedItems = await _client.PopItemsFromSet<string>(set, numberOfItems);
            var leftItems = await _client.GetSetMembers<string>(set);

            //Assert
            setItems.Should().Contain(removedItems);
            leftItems.Should().NotContain(removedItems);
            removedItems.Count().Should().Be(Math.Min(numberOfItems, setItems.Count));
            leftItems.Count().Should().Be(Math.Max(setItems.Count - numberOfItems, 0));
        }

    }
}
