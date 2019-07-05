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
        Task<RedisLowLevelRespons> Parse(StreamReader stream);
    }

    public class SimpleStringParser : IPrefixParser
    {
        public async Task<RedisLowLevelRespons> Parse(StreamReader stream)
        {
            var response = await stream.ReadLineAsync();
            return new RedisLowLevelRespons
            {
                Message = response,
                ResponseType = RedisResponse.SimpleString
            };
        }
    }

    public class ErrorParser : IPrefixParser
    {
        public async Task<RedisLowLevelRespons> Parse(StreamReader stream)
        {
            var response = await stream.ReadLineAsync();
            return new RedisLowLevelRespons
            {
                Message = response,
                ResponseType = RedisResponse.Error
            };
        }
    }

    public class BulkStringParser : IPrefixParser
    {
        public async Task<RedisLowLevelRespons> Parse(StreamReader stream)
        {
            var responseLength = await stream.ReadLineAsync();
            var response = await stream.ReadLineAsync();
            return new RedisLowLevelRespons
            {
                Message = response,
                ResponseType = RedisResponse.BulkString
            };
        }
    }

    public class IntegerParser : IPrefixParser
    {
        public async Task<RedisLowLevelRespons> Parse(StreamReader stream)
        {
            var response = await stream.ReadLineAsync();
            return new RedisLowLevelRespons
            {
                Message = response,
                ResponseType = RedisResponse.Integer
            };
        }
    }

    public class ArrayParser : IPrefixParser
    {
        private readonly Dictionary<RedisResponse, IPrefixParser> _prefixParsers;
        public ArrayParser(Dictionary<RedisResponse, IPrefixParser> prefixParsers)
        {
            _prefixParsers = prefixParsers;
        }

        public async Task<RedisLowLevelRespons> Parse(StreamReader stream)
        {
            var builder = new StringBuilder();
            var length = int.Parse(await stream.ReadLineAsync());
            for (var i = 0; i < length; i++)
            {
                var prefixChar = (RedisResponse)stream.Read();
                var parsedResponse = await _prefixParsers[prefixChar].Parse(stream);
                builder.Append($"{parsedResponse.Message} ");
            }
            return new RedisLowLevelRespons
            {
                Message = builder.ToString().TrimEnd(),
                ResponseType = RedisResponse.Array
            };
        }
    }
}
