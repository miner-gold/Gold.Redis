using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Parsers.PrefixParsers
{
    public class ArrayParser : IPrefixParser
    {
        private readonly Dictionary<char, IPrefixParser> _prefixParsers;
        public ArrayParser(Dictionary<char, IPrefixParser> prefixParsers)
        {
            _prefixParsers = prefixParsers;
        }

        public async Task<Response> Parse(StreamReader stream)
        {
            var arguments = new List<Response>();
            var length = int.Parse(await stream.ReadLineAsync());
            for (var i = 0; i < length; i++)
            {
                var prefixChar = (char)stream.Read();
                var response = await _prefixParsers[prefixChar].Parse(stream);
                arguments.Add(response);
            }

            return new ArrayResponse
            {
                Responses = arguments
            };
        }
    }
}