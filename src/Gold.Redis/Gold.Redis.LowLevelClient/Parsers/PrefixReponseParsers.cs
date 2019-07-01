using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public interface IPrefixParser
    {
        Task<string> Parse(StreamReader stream);
    }

    public class SimpleStringParser : IPrefixParser
    {
        public async Task<string> Parse(StreamReader stream)
        {
            return await stream.ReadLineAsync();
        }
    }

    public class ErrorParser : IPrefixParser
    {
        public async Task<string> Parse(StreamReader stream)
        {
            return await stream.ReadLineAsync();
        }
    }

    public class BulkStringParser : IPrefixParser
    {
        public async Task<string> Parse(StreamReader stream)
        {
            var responseLength = await stream.ReadLineAsync();
            return await stream.ReadLineAsync();
        }
    }

    public class IntegerParser : IPrefixParser
    {
        public async Task<string> Parse(StreamReader stream)
        {
            return await stream.ReadLineAsync();
        }
    }

    public class ArrayParser : IPrefixParser
    {
        private readonly Dictionary<char, IPrefixParser> _prefixParsers;
        public ArrayParser(Dictionary<char, IPrefixParser> prefixParsers)
        {
            _prefixParsers = prefixParsers;
        }

        public async Task<string> Parse(StreamReader stream)
        {
            var builder = new StringBuilder();
            var length = int.Parse(await stream.ReadLineAsync());
            for (var i = 0; i < length; i++)
            {
                var prefixChar = (char)stream.Read();
                builder.Append($"{await _prefixParsers[prefixChar].Parse(stream)} ");
            }

            return builder.ToString().TrimEnd();
        }
    }
}
