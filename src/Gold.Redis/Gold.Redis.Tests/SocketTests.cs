<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.Common.Interfaces.Communication;
using Gold.Redis.Common.Models.Configuration;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Parsers;
using Moq;
using NUnit.Framework;

namespace Gold.Redis.Tests
{
    [TestFixture]
    public class SocketTests
    {
        [Test]
        public void CreateSocketContainer_ShouldFreeSocketWhenFinished()
        {
            // Arrange
            var socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            var connectionsContainerMock = new Mock<IConnectionsContainer>();
            var socketContainer = new SocketContainer(socket,
                connectionsContainerMock.Object);

            // Act
            using (socketContainer)
            {

            }

            //Assert
            connectionsContainerMock.Verify(container => container.FreeSocket(socket), Times.Once);
        }

        [Test]
        public async Task CreateSocket_FreeTheSocket_VerifyThatTheSameSocketReturned()
        {
            // Arrange
            var connectionsContainer = new SocketsConnectionsContainer(new RedisConnectionConfiguration
            { Host = "localhost", MaxConnections = 1 });

            // Act
            var socketContainer = await connectionsContainer.GetSocket();
            var socket = socketContainer.Socket;
            socketContainer.Dispose();
            var secondContainer = await connectionsContainer.GetSocket();

            // Assert
            secondContainer.Socket.Should().BeSameAs(socket);
        }

        [Test]
        public async Task CreateSocket_WaitUntilSocketIsFree_VerifyThatTheSameSocketReturned()
        {
            // Arrange
            var connectionsContainer = new SocketsConnectionsContainer(new RedisConnectionConfiguration
            { Host = "localhost", MaxConnections = 1 });

            // Act
            var socketContainer = await connectionsContainer.GetSocket();
            var socket = socketContainer.Socket;
            var disposeTask = Task.Run(async () =>
            {
                await Task.Delay(200);
                socketContainer.Dispose();
            });
            var secondContainer = await connectionsContainer.GetSocket();

            // Assert
            disposeTask.IsCompleted.Should().BeTrue();
            secondContainer.Socket.Should().BeSameAs(socket);
        }
    }
}

=======
﻿using System.Net.Sockets;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.Common.Interfaces.Communication;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace Gold.Redis.Tests
{
    [TestFixture]
    public class SocketTests
    {
        private Mock<IRedisAuthenticator> _authenticator;

        [SetUp]
        public void SetUp()
        {
            _authenticator = new Mock<IRedisAuthenticator>();
        }

        [Test]
        public void CreateSocketContainer_ShouldFreeSocketWhenFinished()
        {
            // Arrange
            var socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            var connectionsContainerMock = new Mock<IConnectionsContainer>();
            var socketContainer = new SocketContainer(socket,
                connectionsContainerMock.Object);

            // Act
            using (socketContainer)
            {

            }

            //Assert
            connectionsContainerMock.Verify(container => container.FreeSocket(socket), Times.Once);
        }

        [Test]
        public async Task CreateSocket_FreeTheSocket_VerifyThatTheSameSocketReturned()
        {
            // Arrange
            var configuration = RedisConfigurationLoader.GetConfiguration();
            configuration.MaxConnections = 1;
            _authenticator.Setup(auth => auth.TryAuthenticate(It.IsAny<Socket>(), It.IsAny<string>())).ReturnsAsync(true);
            var connectionContainer = new SocketsConnectionsContainer(configuration, _authenticator.Object);

            // Act
            var socketContainer = await connectionContainer.GetSocket();
            var socket = socketContainer.Socket;
            socketContainer.Dispose();
            var secondContainer = await connectionContainer.GetSocket();

            // Assert
            secondContainer.Socket.Should().BeSameAs(socket);
        }

        [Test]
        public async Task CreateSocket_WaitUntilSocketIsFree_VerifyThatTheSameSocketReturned()
        {

            // Arrange
            var configuration = RedisConfigurationLoader.GetConfiguration();
            configuration.MaxConnections = 1;
            _authenticator.Setup(auth => auth.TryAuthenticate(It.IsAny<Socket>(), It.IsAny<string>())).ReturnsAsync(true);
            var connectionContainer = new SocketsConnectionsContainer(configuration, _authenticator.Object);

            // Act
            var socketContainer = await connectionContainer.GetSocket();
            var socket = socketContainer.Socket;
            var disposeTask = Task.Run(async () =>
            {
                await Task.Delay(200);
                socketContainer.Dispose();
            });
            var secondContainer = await connectionContainer.GetSocket();

            // Assert
            disposeTask.IsCompleted.Should().BeTrue();
            secondContainer.Socket.Should().BeSameAs(socket);
        }
    }
}

>>>>>>> 48e617b... Fixed existing U.T to handle passwords
