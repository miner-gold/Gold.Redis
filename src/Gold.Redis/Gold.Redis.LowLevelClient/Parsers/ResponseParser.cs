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
        private readonly Dictionary<RedisResponse, IPrefixParser> _prefixParsers;
        public ResponseParser(Dictionary<RedisResponse, IPrefixParser> prefixParsers)
        {
            _prefixParsers = prefixParsers;
        }

        public async Task<RedisLowLevelRespons> Parse(StreamReader stream)
        {
            var firstChar = (RedisResponse)stream.Read();
            return await _prefixParsers[firstChar].Parse(stream);
        }
    }


}
