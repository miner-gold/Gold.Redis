using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.Tests.Helpers;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "SetAdd")]
    public class SetAddCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [Test]
        public async Task SetAdd_ShouldCreateSetAndHaveOneItem_WhenCallingSetAddOnNewSetWithSingleItem()
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            var setItem = Guid.NewGuid().ToString();

            //Act
            var added = await _client.SetAdd(setKey, setItem);
            var setExists = await _client.IsKeyExists(setKey);
            var setItems = await _client.GetSetMembers<string>(setKey);

            //Assert
            added.Should().BeTrue();
            setExists.Should().BeTrue();
            setItems.ElementAt(0).Should().Be(setItem);
            setItems.Count().Should().Be(1);
        }

        [TestCase("1", "2", "3")]
        [TestCase(1, 2, 3)]
        [TestCase('1', '2', '3')]
        [Test]
        public async Task SetAdd_AddMultiplierItems_ShouldAddAllItems(params object[] items)
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();

            //Act
            var added = await _client.SetAddMultiple<object>(setKey, items);
            var setExists = await _client.IsKeyExists(setKey);
            var setItems = await _client.GetSetMembers<object>(setKey);

            //Assert
            added.Should().BeTrue();
            setExists.Should().BeTrue();
            setItems.Count().Should().Be(items.Length);
            var setItemsAsStrings = setItems.Select(item => item.ToString());
            setItemsAsStrings.Should().Contain(items.Select(item => item.ToString()));
        }

        [Test]
        public async Task SetAdd_AddItemToExistingSet_ShouldHaveAllItems()
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            await _client.SetAdd<string>(setKey, "some_item");
            var randomItemsToAdd = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid().ToString());

            //Act
            await _client.SetAddMultiple<string>(setKey, randomItemsToAdd);
            var items = await _client.GetSetMembers<string>(setKey);

            //Assert
            items.Count().Should().Be(11);
            items.Should().Contain("some_item");
        }
    }
}
