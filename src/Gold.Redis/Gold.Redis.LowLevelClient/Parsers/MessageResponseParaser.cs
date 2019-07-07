using System;
using System.Collections.Generic;
using System.Linq;
using Gold.Redis.Common;
using Gold.Redis.Common.Interfaces.Parsers;
using Gold.Redis.Common.Models;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public class MessageParser : IMessageParser
    {
        private readonly IEnumerable<IMessageParser> _parsers;

        public MessageParser(IEnumerable<IMessageParser> parsers)
        {
            _parsers = parsers;
        }

        public bool Test(RedisLowLevelResponse response)
        {
            var correctParser = _parsers.FirstOrDefault(parser => parser.Test(response));
            return correctParser != null;
        }

        public KeyValuePair<MessageType, IDictionary<string, string>> Parse(RedisLowLevelResponse response)
        {
            var correctParser = _parsers.FirstOrDefault(parser => parser.Test(response));
            if (correctParser == null) throw new MissingMethodException("correct parser was not found");
            return correctParser.Parse(response);
        }
    }
}
