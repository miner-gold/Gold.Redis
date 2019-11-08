using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "GetSetMembers")]
    public class GetSetMembersCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [Test]
        public async Task GetSetMembers_ShouldReturnAllSetItems()
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            var items = Enumerable.Range(0, 40).Select(i => Guid.NewGuid().ToString()).ToList();
            await _client.SetAddMultiple<string>(setKey, items);

            //Act
            var setItems = (await _client.GetSetMembers<string>(setKey)).ToList();

            //Assert
            setItems.Count().Should().Be(items.Count());
            setItems.Should().Contain(items);
        }

        [Test]
        public async Task GetSetMembers_ShouldEmptyList_WhenSetDoesNotExists()
        {
            //Act
            var setItems = await _client.GetSetMembers<string>(Guid.NewGuid().ToString());

            //Assert
            setItems.Should().BeEmpty();
        }

    }
}
