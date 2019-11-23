using System.Net.Sockets;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Interfaces.Communication;
using Gold.Redis.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace Gold.Redis.Tests
{
    [TestFixture(Category = "Socket")]
    public class SocketTests
    {
        private Mock<IRedisAuthenticator> _authenticator;
        private Mock<ISocketConnector> _connector;

        [SetUp]
        public void SetUp()
        {
            _authenticator = new Mock<IRedisAuthenticator>();
            _connector = new Mock<ISocketConnector>();
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
            connectionsContainerMock.Verify(container => container.FreeSocket(socketContainer), Times.Once);
        }

        [Test]
        public async Task CreateSocket_FreeTheSocket_VerifyThatTheSameSocketReturned()
        {
            // Arrange
            var configuration = RedisConfigurationLoader.GetConfiguration();
            configuration.MaxConnections = 1;
            _authenticator.Setup(auth => auth.TryAuthenticate(It.IsAny<ISocketContainer>(), It.IsAny<string>())).ReturnsAsync(true);
            var connectionContainer = new SocketsConnectionsContainer(configuration, _authenticator.Object, _connector.Object);

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
            _authenticator.Setup(auth => auth.TryAuthenticate(It.IsAny<ISocketContainer>(), It.IsAny<string>())).ReturnsAsync(true);
            var connectionContainer = new SocketsConnectionsContainer(configuration, _authenticator.Object, _connector.Object);

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
