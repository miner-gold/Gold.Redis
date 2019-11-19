using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.Common;
using Gold.Redis.Common.Configuration;
using Gold.Redis.Common.Utils;
using Gold.Redis.HighLevelClient.Db;
using Gold.Redis.HighLevelClient.Interfaces;
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
    public class WorkerDbIntegrationTests
    {
        private IRedisDb _pipelineClient;
        private IRedisDb _singleClient;
        [OneTimeSetUp]
        public void OneTimeSetUp()
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

            //For the CI to run without the need of configuration file
            if (configuration.Pipeline == null)
            {
                configuration.UsePiplining = true;
                configuration.Pipeline = new RedisPipelineConfiguration();
            }

            var socketCommandExecutor = new SocketCommandExecutor(new RequestBuilder(), parsers);
            var authenticator = new RedisSocketAuthenticator(socketCommandExecutor);
            var connectionContainer = new SocketsConnectionsContainer(configuration, authenticator);
            var lowLevelClient = new RedisCommandHandler(connectionContainer, socketCommandExecutor);


            var pipelineWorkRunner = new PipelineExecutorWorkRunner(lowLevelClient);
            var worker = new MultiplePipelineWorker<string, Response>(configuration.Pipeline, pipelineWorkRunner);


            SetUpSingleClient(lowLevelClient, responseParser);
            SetUpPipelineClient(worker, responseParser);
        }

        private void SetUpSingleClient(RedisCommandHandler lowLevelClient, JsonResponseParser responseParser)
        {
            var commandExecutor = new RedisSingleCommandExecutor(lowLevelClient);
            var commandExecutorHelper = new RedisCommandExecutorHelper(commandExecutor, responseParser);
            var redisScannerHelper = new RedisScanner(commandExecutor);

            var generalDb = new RedisGeneralOperationsDb(commandExecutor);
            var keyDb = new RedisKeysDb(commandExecutor, responseParser);
            var setDb = new RedisSetDb(commandExecutorHelper, responseParser, redisScannerHelper);

            _singleClient = new RedisDb(generalDb, keyDb, setDb);
        }
        private void SetUpPipelineClient(MultiplePipelineWorker<string, Response> worker, JsonResponseParser responseParser)
        {
            var workerCommandExecutor = new RedisWorkerCommandExecutor(worker);
            var commandExecutorHelper = new RedisCommandExecutorHelper(workerCommandExecutor, responseParser);
            var redisScannerHelper = new RedisScanner(workerCommandExecutor);

            var generalDb = new RedisGeneralOperationsDb(workerCommandExecutor);
            var keyDb = new RedisKeysDb(workerCommandExecutor, responseParser);
            var setDb = new RedisSetDb(commandExecutorHelper, responseParser, redisScannerHelper);

            _pipelineClient = new RedisDb(generalDb, keyDb, setDb);
        }

        [SetUp]
        public async Task SetUp()
        {
            await _pipelineClient.FlushDb();
            await _singleClient.FlushDb();
        }

        [Test]
        public async Task PreformSetAndKeyActionUsingPipeling_ShouldExecuteAllRequests()
        {
            //Arrange
            var tasks = new List<Task>
            {
                _pipelineClient.SetKey("some_key", "some_value"),
                _pipelineClient.SetAdd("some_set", 123),
                _pipelineClient.SetAddMultiple("some_value", new List<string> {"123", "345", "678"}),
                _pipelineClient.SetRemove("some_value", "345"),
                _pipelineClient.SetKey("some_key2", 456)
            };

            //Act
            await Task.WhenAll(tasks);

            //Assert
            (await _pipelineClient.Get<string>("some_key")).Should().Be("some_value");
            (await _pipelineClient.Get<int>("some_key2")).Should().Be(456);
            (await _pipelineClient.GetSetMembers<int>("some_set")).Should().BeEquivalentTo(new[] { 123 });
            (await _pipelineClient.GetSetMembers<string>("some_value")).Should().BeEquivalentTo(new[] { "123", "678" });
        }

        [Test]
        public async Task PreformTest_ShouldDo500PingUsingPipeline_FasterThanWithoutPipelined()
        {
            //Arrange
            var pipelineTasks = Enumerable.Range(0, 500).Select(_ => _pipelineClient.Ping());
            var singleExecutedTasks = Enumerable.Range(0, 500).Select(_ => _pipelineClient.Ping());
            var pipelineStopwatch = new Stopwatch();
            var singleStopwatch = new Stopwatch();

            //Act
            pipelineStopwatch.Start();
            var pipeResults = await Task.WhenAll(pipelineTasks);
            pipelineStopwatch.Stop();

            singleStopwatch.Start();
            var singleResults = await Task.WhenAll(singleExecutedTasks);
            singleStopwatch.Stop();

            var pipelineElapsed = pipelineStopwatch.Elapsed;
            var singleElapsed = singleStopwatch.Elapsed;

            //Assert
            pipelineElapsed.Should().BeLessThan(singleElapsed);
            pipeResults.All(i => i).Should().BeTrue();
            singleResults.All(i => i).Should().BeTrue();
        }
    }
}
