using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.LowLevelClient.Parsers;
using Gold.Redis.Tests.AssertExtensions;
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
            var prefixParsers = new Dictionary<RedisResponseTypes, IPrefixParser>
            {
                {RedisResponseTypes.SimpleString, new SimpleStringParser()},
                {RedisResponseTypes.BulkString, new BulkStringParser()},
                {RedisResponseTypes.Integer, new IntegerParser()},
                {RedisResponseTypes.Error, new ErrorParser() }
            };
            _responseParser = new ResponseParser(prefixParsers
                .Concat(new[]
                    {new KeyValuePair<RedisResponseTypes, IPrefixParser>(RedisResponseTypes.Array, new ArrayParser(prefixParsers))})
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
            result.Should().MessageBe(expectedCommand, RedisResponseTypes.SimpleString);
        }

        [Test]
        public async Task ParseResponse_GetResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = $"{(char)RedisResponseTypes.BulkString}" +
                           $"6{Constants.CrLf}RandomString{Constants.CrLf}";
            var expectedCommand = "RandomString";

            //Act
            var result = await _responseParser.Parse(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(response))));

            //Assert
            result.Should().MessageBe(expectedCommand, RedisResponseTypes.BulkString);
        }

        [Test]
        public async Task ParseResponse_BasicIntResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = $"{(char)RedisResponseTypes.Integer}6{Constants.CrLf}";
            var expectedCommand = "6";

            //Act
            var result = await _responseParser.Parse(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(response))));

            //Assert
            result.Should().MessageBe(expectedCommand, RedisResponseTypes.Integer);
        }

        [Test]
        public async Task ParseResponse_BasicErrorResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = $"{(char)RedisResponseTypes.Error}ErrorMessage{Constants.CrLf}";
            var expectedCommand = "ErrorMessage";

            //Act
            var result = await _responseParser.Parse(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(response))));

            //Assert
            result.Should().MessageBe(expectedCommand, RedisResponseTypes.Error);
        }

        [Test]
        public async Task ParseResponse_BasicStringResponse_ShouldParseSuccessfully()
        {
            //Arrange
            var response = $"{(char)RedisResponseTypes.Array}2{Constants.CrLf}" +
                           $"{(char)RedisResponseTypes.BulkString}3{Constants.CrLf}" +
                           $"foo{Constants.CrLf}" +
                           $"{(char)RedisResponseTypes.BulkString}3{Constants.CrLf}" +
                           $"bar{Constants.CrLf}";
            var expectedCommand = "foo bar";

            //Act
            var result = await _responseParser.Parse(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(response))));

            //Assert
            result.Should().MessageBe(expectedCommand, RedisResponseTypes.Array);
        }
    }
}
