using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "GetRandomItemsFromSet")]
    public class GetRandomItemsFromSetCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(150)]
        [Test]
        public async Task GetRandomItemsFromSet_ShouldReturnRandomItems_WithNoDoubles(int numberOfItems)
        {
            //Arrange
            var set = Guid.NewGuid().ToString();
            var setItems = Enumerable.Range(0, 100).Select(i => Guid.NewGuid().ToString()).ToList();
            await _client.SetAddMultiple<string>(set, setItems);

            //Act
            var randomItems = await _client.GetRandomItemsFromSet<string>(set, numberOfItems);
            var leftItems = await _client.GetSetMembers<string>(set);

            //Assert
            setItems.Should().Contain(randomItems);
            randomItems.Count().Should().Be(Math.Min(numberOfItems, setItems.Count));
            leftItems.Should().BeEquivalentTo(setItems);
        }

        [Test]
        public async Task GetRandomItemsFromSet_ShouldReturnSameItemFiveTimes_WhereGetWithMultiple()
        {
            //Arrange
            var set = Guid.NewGuid().ToString();
            var setItems = Enumerable.Range(0, 1).Select(i => Guid.NewGuid().ToString()).ToList();
            await _client.SetAddMultiple<string>(set, setItems);

            //Act
            var randomItems = (await _client.GetRandomItemsFromSet<string>(set, 5, allowMultiple: true)).ToList();

            //Assert
            randomItems.Count().Should().Be(5);
            randomItems.Distinct().Count().Should().Be(1);
            randomItems.Distinct().ElementAt(0).Should().Be(setItems[0]);
        }

        [Test]
        public async Task GetRandomItemFromSet_ShouldReturnNull_WhenSetDoesNotExists()
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();

            //Act
            var item = await _client.GetRandomItemFromSet<string>(setKey);

            //Assert
            Assert.Null(item);
        }

        [Test]
        public async Task GetRandomItemsFromSet_ShouldReturnEmptyList_WhenSetDoesNotExists()
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();

            //Act
            var items = await _client.GetRandomItemsFromSet<string>(setKey,10);

            //Assert
            items.Should().BeEmpty();
        }
    }
}
