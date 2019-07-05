using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.Common.Interfaces.Parsers;
using Gold.Redis.Common.Models;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public class ResponseParser : IResponseParser
    {
        private readonly Dictionary<RedisResponseTypes, IPrefixParser> _prefixParsers;
        public ResponseParser(Dictionary<RedisResponseTypes, IPrefixParser> prefixParsers)
        {
            _prefixParsers = prefixParsers;
        }

        public async Task<RedisLowLevelResponse> Parse(StreamReader stream)
        {
            var firstChar = (RedisResponseTypes)stream.Read();
            return await _prefixParsers[firstChar].Parse(stream);
        }
    }


}
