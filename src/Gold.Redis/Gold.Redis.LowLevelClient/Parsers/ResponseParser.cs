using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.Common.Interfaces.Parsers;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public class ResponseParser : IResponseParser
    {
        private readonly Dictionary<char, IPrefixParser> _prefixParsers;
        public ResponseParser(Dictionary<char, IPrefixParser> prefixParsers)
        {
            _prefixParsers = prefixParsers;
        }

        public async Task<string> Parse(StreamReader stream)
        {
            var firstChar = (char)stream.Read();
            return await _prefixParsers[firstChar].Parse(stream);
        }
    }


}
