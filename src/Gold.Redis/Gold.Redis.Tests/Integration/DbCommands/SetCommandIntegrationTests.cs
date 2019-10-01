using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.HighLevelClient.Utils;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Gold.Redis.Tests.Integration.DbCommands
{
    [TestFixture(Category = "Set")]
    public class SetCommandIntegrationTests : RedisDataBaseClientIntegrationTestsBase
    {
        [SetUp]
        public async Task SetUp() => await TestsSetUp();

        [Test]
        public async Task Set_WithTimeout_ShouldHaveTheKeyForTwoSecondsAndThenLossIs()
        {
            //Arrange
            var expiration = TimeSpan.FromSeconds(2);
            var randomKey = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomKey, expiration);
            var firstCheck = await _client.IsKeyExists(randomKey);
            await Task.Delay(expiration);
            var secondCheck = await _client.IsKeyExists(randomKey);

            //Assert
            firstCheck.Should().BeTrue();
            secondCheck.Should().BeFalse();
        }

        [TestCase(KeyAssertion.MustNotExists)]
        [TestCase(KeyAssertion.MustExist)]
        [Test]
        public async Task Set_WithRestriction_OnlyInsertWhenTheRestrictionIsMet(KeyAssertion assertion)
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomKey);
            var setResultAfterKeyExists = await _client.SetKey(randomKey, randomKey, null, assertion);

            //Assert
            setResultAfterKeyExists.Should().Be(assertion == KeyAssertion.MustExist);
        }
    }
}
