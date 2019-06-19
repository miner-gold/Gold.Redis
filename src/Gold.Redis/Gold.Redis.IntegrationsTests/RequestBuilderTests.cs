using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Gold.Redis.LowLevelClient.Parsers;
using NUnit.Framework;

namespace Gold.Redis.IntegrationsTests
{
    [TestFixture]
    public class RequestBuilderTests
    {
        private RequestBuilder _requestBuilder;

        [SetUp]
        public void SetUp()
        {
            _requestBuilder = new RequestBuilder();
        }

        [Test]
        public void CreateCommand_BasicSetCommand_ShouldCreateSuccessfully()
        {
            //Arrange 
            var command = "SET key 6";
            var expectedCommand = "*3\r\n$3\r\nSET\r\n$3\r\nkey\r\n$1\r\n6\r\n";

            //Act
            var result = _requestBuilder.Build(command);

            //Assert
            result.Should().Be(expectedCommand);
        }
    }
}
