using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;

namespace Gold.Redis.LowLevelClient.Parsers.PrefixParsers
{
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