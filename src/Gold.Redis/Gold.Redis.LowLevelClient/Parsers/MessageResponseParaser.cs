using System.Collections.Generic;
using System.Linq;
using Gold.Redis.Common;
using Gold.Redis.Common.Interfaces.Parsers;
using Gold.Redis.Common.Models;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public class MessageParser : IMessageResponseParser
    {
        private readonly IEnumerable<IMessageParser> _parsers;

        public MessageParser(IEnumerable<IMessageParser> parsers)
        {
            _parsers = parsers;
        }

        public KeyValuePair<MessageType, IDictionary<string, string>> Parse(RedisLowLevelResponse response)
        {
            var correctParser = _parsers.First(parser => parser.Test(response));
            return correctParser.Parse(response);
        }
    }
}
