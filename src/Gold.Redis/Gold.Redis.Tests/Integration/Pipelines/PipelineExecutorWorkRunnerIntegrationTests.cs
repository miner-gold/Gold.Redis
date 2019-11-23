using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.Common;
using Gold.Redis.HighLevelClient.Pipeline;
using Gold.Redis.HighLevelClient.ResponseParsers;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Parsers;
using Gold.Redis.LowLevelClient.Parsers.PrefixParsers;
using Gold.Redis.LowLevelClient.Responses;
using Gold.Redis.Tests.Helpers;
using NUnit.Framework;

namespace Gold.Redis.Tests.Integration.Pipelines
{
    [TestFixture(Category = "Pipelining")]
    public class PipelineExecutorWorkRunnerIntegrationTests
    {
        private PipelineExecutorWorkRunner _workRunner;

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
            var parsers = new ResponseParser(prefixParsers
                .Concat(new[]
                    {new KeyValuePair<char, IPrefixParser>(CommandPrefixes.Array, new ArrayParser(prefixParsers))})
                .ToDictionary(d => d.Key, d => d.Value));
            var responseParser = new JsonResponseParser();

            var configuration = RedisConfigurationLoader.GetConfiguration();
            var socketCommandExecutor = new SocketCommandExecutor(new RequestBuilder(), parsers);
            var socketConnector = new SocketConnectorWithRetries(configuration);
            var authenticator = new RedisSocketAuthenticator(socketCommandExecutor);
            var connectionContainer = new SocketsConnectionsContainer(configuration, authenticator, socketConnector);
            var lowLevelClient = new RedisCommandHandler(connectionContainer, socketCommandExecutor, configuration);

            _workRunner = new PipelineExecutorWorkRunner(lowLevelClient);
        }

        [Test]
        public async Task ExecuteWork_PingPingPing_ShouldReturnPongPongPong()
        {
            //Arrange
            var commands = new[] { "PING", "PING", "PING" };

            //Act
            var responses = await _workRunner.ExecuteWork(commands);

            //Assert
            responses.Count.Should().Be(commands.Length);
            foreach (var response in responses)
            {
                (response as SimpleStringResponse).Response.Should().Be("PONG");
            }

        }

        [Test]
        public async Task ExecuteWork_SetKey_GetKey_RemoveKeyShouldReturnOkKeyValueOk()
        {
            //Arrange
            var commands = new[] { "SET key \"10\"", "GET key", "DEL key" };

            //Act
            var responses = await _workRunner.ExecuteWork(commands);

            var responsedValues = responses.ToArray();

            //Assert
            var first = (responsedValues[0] as SimpleStringResponse).Response == Constants.OkResponse;
            var second = (responsedValues[1] as BulkStringResponse).Response == "\"10\"";
            var third = (responsedValues[2] as IntResponse).Response == 1;

            first.Should().BeTrue();
            second.Should().BeTrue();
            third.Should().BeTrue();
        }

    }
}
