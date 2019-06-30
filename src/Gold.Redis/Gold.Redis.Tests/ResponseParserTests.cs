using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Gold.Redis.Common;
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
        public async Task ParseResponse_BasicOkResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = "+OK\r\n";
            var expectedCommand = "OK";

            //Act
            var result = await _responseParser.Parse(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(response))));

            //Assert
            result.Should().Be(expectedCommand);
        }

        [Test]
        public async Task ParseResponse_GetResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = $"{CommandPrefixes.BulkString}" +
                           $"6{Constants.CrLf}RandomString{Constants.CrLf}";
            var expectedCommand = "RandomString";

            //Act
            var result = await _responseParser.Parse(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(response))));

            //Assert
            result.Should().Be(expectedCommand);
        }
    }
}
