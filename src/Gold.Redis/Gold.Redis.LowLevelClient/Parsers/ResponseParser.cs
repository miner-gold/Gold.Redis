﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public class ResponseParser : IResponseParser
    {
        private readonly Dictionary<char, IPrefixParser> _prefixParsers;
        public ResponseParser(Dictionary<char, IPrefixParser> prefixParsers)
        {
            _prefixParsers = prefixParsers;
        }

        public async Task<Response> Parse(StreamReader stream)
        {
            var firstChar = (char)stream.Read();
            return await _prefixParsers[firstChar].Parse(stream);
        }
    }


}
