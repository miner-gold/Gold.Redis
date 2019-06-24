using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Gold.Redis.Common;
using Gold.Redis.LowLevelClient.Parsers;
using NUnit.Framework;

namespace Gold.Redis.Tests
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
        public void CreateCommand_BasicSetCommand_ShouldReturnCorrectString()
        {
            //Arrange 
            var command = "SET key 6";
            var expectedCommand = $"*3{Constants.CrLf}$3{Constants.CrLf}" +
                                  $"SET{Constants.CrLf}" +
                                  $"$3{Constants.CrLf}key{Constants.CrLf}" +
                                  $"$1{Constants.CrLf}6{Constants.CrLf}";

            //Act
            var result = _requestBuilder.Build(command);

            //Assert
            result.Should().Be(expectedCommand);
        }

        [Test]
        public void CreateCommand_PingCommand_ShouldReturnCorrectString()
        {
            //Arrange 
            var command = "PING";
            var expectedCommand = $"*1{Constants.CrLf}" +
                                  $"$4{Constants.CrLf}" +
                                  $"PING{Constants.CrLf}";

            //Act
            var result = _requestBuilder.Build(command);

            //Assert
            result.Should().Be(expectedCommand);
        }
    }
}
