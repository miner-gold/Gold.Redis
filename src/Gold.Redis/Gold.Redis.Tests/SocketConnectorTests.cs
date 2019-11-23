using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.Common.Configuration;
using Gold.Redis.Common.Exceptions;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Interfaces.Communication;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Gold.Redis.Tests
{
    [TestFixture(Category = "Socket")]
    public class SocketConnectorTests
    {
        private Mock<ISocketContainer> _socket;

        [SetUp]
        public void Setup()
        {
            _socket = new Mock<ISocketContainer>();
        }

        [Test]
        public async Task Connect_ShouldTryToConnectSixTime_AndWait_WhenSocketConnectionFails()
        {
            //Arrange
            var config = new RedisConnectionConfiguration
            {
                ConnectionFailedWaitTime = TimeSpan.FromSeconds(2),
                ConnectionRetries = 6,
                ConnectTimeout = TimeSpan.FromSeconds(1)
            };
            var connector = new SocketConnectorWithRetries(config);

            _socket.Setup(socket => socket.ConnectAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.Run(() => Thread.Sleep(50000)));

            //Act
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Exception exceptionResult = new Exception();
            try
            {
                await connector.ConnectSocket(_socket.Object);
            }
            catch (GoldRedisConnectionTimeoutException timeoutException)
            {
                exceptionResult = timeoutException;
            }
            catch (Exception ex)
            {
                exceptionResult = ex;
            }
            watch.Stop();

            //Assert
            exceptionResult.Should().BeOfType<GoldRedisConnectionTimeoutException>();
            watch.Elapsed.Should().BeCloseTo(
                (config.ConnectTimeout * config.ConnectionRetries) +
                (config.ConnectionFailedWaitTime * config.ConnectionRetries),
                TimeSpan.FromSeconds(1));

            _socket.Verify(socket => socket.ConnectAsync(It.IsAny<string>(), It.IsAny<int>()),
                Times.Exactly(config.ConnectionRetries));
        }

        [Test]
        public async Task Connect_ShouldWaitUntilTheTaskConnectIsDone_WhenConfigurationsAreZeroWaitTime()
        {
            //Arrange
            var config = new RedisConnectionConfiguration
            {
                ConnectionFailedWaitTime = TimeSpan.FromSeconds(2),
                ConnectionRetries = 6,
                ConnectTimeout = TimeSpan.Zero
            };

            var connectTime = TimeSpan.FromSeconds(1.5);
            var connector = new SocketConnectorWithRetries(config);

            _socket.Setup(socket => socket.ConnectAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.Run(() => Thread.Sleep(connectTime)));

            //Act
            Stopwatch watch = new Stopwatch();
            watch.Start();
            await connector.ConnectSocket(_socket.Object);
            watch.Stop();

            //Assert
            watch.Elapsed.Should().BeCloseTo(connectTime, TimeSpan.FromSeconds(0.5));

            _socket.Verify(socket => socket.ConnectAsync(It.IsAny<string>(), It.IsAny<int>()),
                Times.Exactly(1));
        }
    }
}
