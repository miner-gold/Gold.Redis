using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var prefixParsers = new Dictionary<char, IPrefixParser>
            {
                {CommandPrefixes.SimpleString, new SimpleStringParser()},
                {CommandPrefixes.BulkString, new BulkStringParser()},
                {CommandPrefixes.Integer, new IntegerParser()},
                {CommandPrefixes.Error, new ErrorParser() }
            };
            _responseParser = new ResponseParser(prefixParsers
                .Concat(new[]
                    {new KeyValuePair<char, IPrefixParser>(CommandPrefixes.Array, new ArrayParser(prefixParsers))})
                .ToDictionary(d => d.Key, d => d.Value));
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

        [Test]
        public async Task ParseResponse_BasicIntResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = $"{CommandPrefixes.Integer}6{Constants.CrLf}";
            var expectedCommand = "6";

            //Act
            var result = await _responseParser.Parse(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(response))));

            //Assert
            result.Should().Be(expectedCommand);
        }

        [Test]
        public async Task ParseResponse_BasicErrorResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = $"{CommandPrefixes.Error}ErrorMessage{Constants.CrLf}";
            var expectedCommand = "ErrorMessage";

            //Act
            var result = await _responseParser.Parse(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(response))));

            //Assert
            result.Should().Be(expectedCommand);
        }

        [Test]
        public async Task ParseResponse_BasicStringResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = $"{CommandPrefixes.Array}2{Constants.CrLf}" +
                           $"{CommandPrefixes.BulkString}3{Constants.CrLf}" +
                           $"foo{Constants.CrLf}" +
                           $"{CommandPrefixes.BulkString}3{Constants.CrLf}" +
                           $"bar{Constants.CrLf}";
            var expectedCommand = "foo bar";

            //Act
            var result = await _responseParser.Parse(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(response))));

            //Assert
            result.Should().Be(expectedCommand);
        }
    }
}
