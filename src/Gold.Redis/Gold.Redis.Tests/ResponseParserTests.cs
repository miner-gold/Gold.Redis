using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Gold.Redis.LowLevelClient.Parsers;
using NUnit.Framework;

namespace Gold.Redis.Tests
{
    [TestFixture]
    public class ResponseParserTests
    {
        private ResponseParser _responseParser;

        [SetUp]
        public void SetUp()
        {
            _responseParser = new ResponseParser();
        }

        [Test]
        public void ParseResponse_BasicOkResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = "+OK\r\n";
            var expectedCommand = "+OK";

            //Act
            var result = _responseParser.Parse(new MemoryStream(Encoding.ASCII.GetBytes(response)));

            //Assert
            result.Should().Be(expectedCommand);
        }
    }
}
