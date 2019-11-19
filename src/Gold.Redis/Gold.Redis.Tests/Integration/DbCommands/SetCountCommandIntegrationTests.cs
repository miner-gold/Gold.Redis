using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "SetCount")]
    public class SetCountCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [Test]
        public async Task SetCount_ShouldReturnZero_WhenSetDoesNotExists()
        {
            //Act
            var result = await _client.SetCount(Guid.NewGuid().ToString());

            //Assert
            result.Should().Be(0);
        }
        [TestCase(1, 2, 3, 4, 5, 6, 7, 8)]
        [TestCase(1, 2, 3)]
        [TestCase(-1, -2, -3, -4)]
        [TestCase(1, 1, 2, 2)]
        [Test]
        public async Task SetCount_ShouldReturnNumberOfDistinctElements_WhenSetInsertedWithNumberOfItems(params int[] numbers)
        {
            //Arrange
            var setKey = Guid.NewGuid().ToString();
            var distinctNumbersCount = numbers.Distinct().Count();
            var add = await _client.SetAddMultiple<int>(setKey, numbers);

            //Act
            var result = await _client.SetCount(setKey);

            //Assert
            add.Should().BeTrue();
            result.Should().Be(distinctNumbersCount);
        }
    }
}
