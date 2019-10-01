using FluentAssertions;
using Gold.Redis.Common;
using Gold.Redis.HighLevelClient.Db;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Parsers;
using Gold.Redis.LowLevelClient.Parsers.PrefixParsers;
using Gold.Redis.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.ResponseParsers;
using Gold.Redis.HighLevelClient.Utils;

namespace Gold.Redis.Tests.Integration
{
    [TestFixture]
    public class RedisGeneralOperationsDbIntegrationTests
    {
        private RedisGeneralOperationsDb _client;

        [SetUp]
        public void SetUp()
        {
            var prefixParsers = new Dictionary<char, IPrefixParser>
            {
                {CommandPrefixes.SimpleString, new SimpleStringParser()},
                {CommandPrefixes.BulkString, new BulkStringParser()},
                {CommandPrefixes.Integer, new IntegerParser()},
                {CommandPrefixes.Error, new ErrorParser() }
            };
            var responseParser = new ResponseParser(prefixParsers
                .Concat(new[]
                    {new KeyValuePair<char, IPrefixParser>(CommandPrefixes.Array, new ArrayParser(prefixParsers))})
                .ToDictionary(d => d.Key, d => d.Value));
            var configuration = RedisConfigurationLoader.GetConfiguration();
            var socketCommandExecutor = new SocketCommandExecutor(new RequestBuilder(), responseParser);
            var authenticator = new RedisSocketAuthenticator(socketCommandExecutor);
            var connectionContainer = new SocketsConnectionsContainer(configuration, authenticator);
            var lowLevelClient = new RedisCommandHandler(connectionContainer, socketCommandExecutor);


            var commandExecutor = new RedisCommandsExecutor(lowLevelClient);
            _client = new RedisGeneralOperationsDb(commandExecutor, new JsonResponseParser());
            _client.FlushDb().GetAwaiter().GetResult();
        }

        #region KeyExists
        [Test, Category("IsKeyExists")]
        public async Task IsKeyExist_KeyDoesNotExist_ShouldReturnFalse()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            var result = await _client.IsKeyExists(randomKey);

            //Assert
            result.Should().BeFalse();
        }

        [Test, Category("IsKeyExists")]
        public async Task IsKeyExist_KeyDoesExists_ShouldReturnTrue()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomKey);
            var result = await _client.IsKeyExists(randomKey);

            //Assert
            result.Should().BeTrue();
        }
        #endregion

        #region Get
        [Test, Category("Get")]
        public async Task Get_KeyExists_ShouldReturnTheInsertedValue()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();
            var randomValue = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomValue);
            var result = await _client.Get<string>(randomKey);

            //Assert
            result.Should().Be(randomValue);
        }

        [Test, Category("Get")]
        public async Task Get_KeyDoesNotExists_ShouldReturnNull()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            var result = await _client.Get<string>(randomKey);

            //Assert
            Assert.Null(result);
        }
        #endregion

        #region Set
        [Test, Category("Set")]
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
        [Test, Category("Set")]
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
        #endregion

        #region GetMathcingKeys
        [Test, Category("GetMatchingKeys")]
        public async Task GetMatchingKeys_ShouldReturnAllKeys_WhenPatternIsAll()
        {
            //Arrange
            var pattern = "*";

            //Act
            for (int i = 1; i < 4; i++)
            {
                await _client.SetKey("KEY_" + i, i);
            }

            var keys = await _client.GetMatchingKeys(pattern);

            //Assert
            keys.Count().Should().Be(3);
        }

        [Test, Category("GetMatchingKeys")]
        public async Task GetMatchingKeys_ShouldReturnOnlyKeysThatEndsWithHello_WhenPatternIsEndWithHello()
        {
            //Arrange
            var pattern = "*Hello";

            //Act
            await _client.SetKey("Hello", "1");
            await _client.SetKey("BY", "0");
            await _client.SetKey("blablaHello", "2");
            await _client.SetKey("blablaHello123", "0");
            await _client.SetKey("123Hello", "3");

            var keys = await _client.GetMatchingKeys(pattern);

            //Assert
            keys.Count().Should().Be(3);
        }

        [Test, Category("GetMatchingKeys")]
        public async Task GetMatchingKeys_ShouldReturnEmptyList_WhenNoMatchingKeysExists()
        {
            //Arrange
            var pattern = "*Hello";

            //Act
            await _client.SetKey("BY", "0");
            await _client.SetKey("blablaHello123", "0");

            var keys = await _client.GetMatchingKeys(pattern);

            //Assert
            keys.Count().Should().Be(0);
        }

        [Test, Category("GetMatchingKeys")]
        public async Task GetMatchingKeys_ShouldReturnEmptyList_WhenNoKeysExistsInDb()
        {
            //Arrange
            var pattern = "*";

            //Act
            var keys = await _client.GetMatchingKeys(pattern);

            //Assert
            keys.Count().Should().Be(0);
        }
        #endregion

        #region SetKeyExpire
        [Test, Category("SetKeyExpire")]
        public async Task SetKeyExpire_ShouldHaveTheKeyForFourSecondsAndThenLossIs()
        {
            //Arrange
            var expiration = TimeSpan.FromSeconds(4);
            var randomKey = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomKey);
            await _client.SetKeyExpire(randomKey, expiration);
            var firstCheck = await _client.IsKeyExists(randomKey);
            await Task.Delay(expiration);
            var secondCheck = await _client.IsKeyExists(randomKey);

            //Assert
            firstCheck.Should().BeTrue();
            secondCheck.Should().BeFalse();
        }

        [Test, Category("SetKeyExpire")]
        public async Task SetKeyExpire_ShouldReturnFalse_WhenSettingOnNoneExistingKey()
        {
            //Arrange
            var expiration = TimeSpan.FromSeconds(4);
            var randomKey = Guid.NewGuid().ToString();

            //Act
            var result = await _client.SetKeyExpire(randomKey, expiration);

            //Assert
            result.Should().BeFalse();
        }

        #endregion

        #region DeleteKey
        [Test, Category("DeleteKey")]
        public async Task DeleteKey_ShouldDeleteTheExistingKey()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            await _client.SetKey(randomKey, randomKey);
            var firstCheck = await _client.IsKeyExists(randomKey);
            await _client.DeleteKey(randomKey);
            var secondCheck = await _client.IsKeyExists(randomKey);

            //Assert
            firstCheck.Should().BeTrue();
            secondCheck.Should().BeFalse();
        }

        [Test, Category("DeleteKey")]
        public async Task DeleteKey_ShouldReturnFalse_WhenTryingToDeleteNonExistingKey()
        {
            //Arrange
            var randomKey = Guid.NewGuid().ToString();

            //Act
            var result = await _client.DeleteKey(randomKey);

            //Assert
            result.Should().BeFalse();
        }
        #endregion
    }
}
