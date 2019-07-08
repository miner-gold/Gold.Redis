using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.Common.Models;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public interface IPrefixParser
    {
        Task<RedisLowLevelResponse> Parse(StreamReader stream);
    }

    public class SimpleStringParser : IPrefixParser
    {
        public async Task<RedisLowLevelResponse> Parse(StreamReader stream)
        {
            var response = await stream.ReadLineAsync();
            return new RedisLowLevelResponse
            {
                Message = response,
                ResponseType = RedisResponseTypes.SimpleString
            };
        }
    }

    public class ErrorParser : IPrefixParser
    {
        public async Task<RedisLowLevelResponse> Parse(StreamReader stream)
        {
            var response = await stream.ReadLineAsync();
            return new RedisLowLevelResponse
            {
                Message = response,
                ResponseType = RedisResponseTypes.Error
            };
        }
    }

    public class BulkStringParser : IPrefixParser
    {
        public async Task<RedisLowLevelResponse> Parse(StreamReader stream)
        {
            var responseLength = await stream.ReadLineAsync();
            var response = await stream.ReadLineAsync();
            return new RedisLowLevelResponse
            {
                Message = response,
                ResponseType = RedisResponseTypes.BulkString
            };
        }
    }

    public class IntegerParser : IPrefixParser
    {
        public async Task<RedisLowLevelResponse> Parse(StreamReader stream)
        {
            var response = await stream.ReadLineAsync();
            return new RedisLowLevelResponse
            {
                Message = response,
                ResponseType = RedisResponseTypes.Integer
            };
        }
    }

    public class ArrayParser : IPrefixParser
    {
        private readonly Dictionary<RedisResponseTypes, IPrefixParser> _prefixParsers;
        public ArrayParser(Dictionary<RedisResponseTypes, IPrefixParser> prefixParsers)
        {
            _prefixParsers = prefixParsers;
        }

        public async Task<RedisLowLevelResponse> Parse(StreamReader stream)
        {
            var builder = new StringBuilder();
            var length = int.Parse(await stream.ReadLineAsync());
            for (var i = 0; i < length; i++)
            {
                var prefixChar = (RedisResponseTypes)stream.Read();
                var parsedResponse = await _prefixParsers[prefixChar].Parse(stream);
                builder.Append($"{parsedResponse.Message} ");
            }
            return new RedisLowLevelResponse
            {
                Message = builder.ToString().TrimEnd(),
                ResponseType = RedisResponseTypes.Array
            };
        }
    }
}
