using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Primitives;
using Gold.Redis.Common.Configuration;
using Gold.Redis.Common.Exceptions;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Interfaces.Communication;
using Gold.Redis.LowLevelClient.Responses;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Gold.Redis.Tests
{
    [TestFixture(Category = "RedisCommandHandler")]
    public class RedisCommandHandlerTests
    {
        private Mock<IConnectionsContainer> _connectionContainer;
        private Mock<ISocketCommandExecutor> _commandExecutor;

        [SetUp]
        public void Setup()
        {
            _connectionContainer = new Mock<IConnectionsContainer>();
            _commandExecutor = new Mock<ISocketCommandExecutor>();
        }

        [Test]
        public async Task ExecuteCommand_ShouldThrowTimeoutException_WhenCommandSurpassTimeout()
        {
            //Arrange
            var config = new RedisConnectionConfiguration
            {
                RequestTimeout = TimeSpan.FromSeconds(3)
            };
            var handler = new RedisCommandHandler(_connectionContainer.Object, _commandExecutor.Object, config);

            _commandExecutor.Setup(executor => executor.ExecuteCommand<SimpleStringResponse>(
                    It.IsAny<ISocketContainer>(),
                    It.IsAny<string>()))
                .Returns(Task.Run(() =>
                {
                    Thread.Sleep(500000);
                    return new SimpleStringResponse();
                }));

            //Act
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Exception exceptionResult = new Exception();
            try
            {
                await handler.ExecuteCommand<SimpleStringResponse>(It.IsAny<string>());
            }
            catch (GoldRedisRequestTimeoutException timeoutException)
            {
                exceptionResult = timeoutException;
            }
            catch (Exception ex)
            {
                exceptionResult = ex;
            }
            watch.Stop();

            //Assert
            exceptionResult.Should().BeOfType<GoldRedisRequestTimeoutException>();
            watch.Elapsed.Should().BeCloseTo(
                config.RequestTimeout,
                TimeSpan.FromSeconds(1));
        }

        [Test]
        public async Task ExecuteCommand_ShouldTakeTheActionTheTime_WhenConfigurationDidNotPutRequestTimeout()
        {
            //Arrange
            var config = new RedisConnectionConfiguration
            {
                RequestTimeout = TimeSpan.Zero
            };
            var handler = new RedisCommandHandler(_connectionContainer.Object, _commandExecutor.Object, config);
            var requestTime = TimeSpan.FromSeconds(1);


            _commandExecutor.Setup(executor => executor.ExecuteCommand<SimpleStringResponse>(
                    It.IsAny<ISocketContainer>(),
                    It.IsAny<string>()))
                .Returns(Task.Run(() =>
                {
                    Thread.Sleep(requestTime);
                    return new SimpleStringResponse();
                }));

            //Act
            Stopwatch watch = new Stopwatch();
            watch.Start();
            await handler.ExecuteCommand<SimpleStringResponse>(It.IsAny<string>());
            watch.Stop();

            //Assert
            watch.Elapsed.Should().BeCloseTo(
                requestTime,
                TimeSpan.FromSeconds(1));
        }

    }
}
