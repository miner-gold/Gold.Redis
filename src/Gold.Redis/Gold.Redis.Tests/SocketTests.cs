using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
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
    }
}
