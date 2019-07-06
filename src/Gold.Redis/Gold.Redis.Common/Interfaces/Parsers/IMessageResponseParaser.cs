using System.Collections.Generic;
using Gold.Redis.Common.Models;

namespace Gold.Redis.Common.Interfaces.Parsers
{
    public interface IMessageResponseParser
    {
        KeyValuePair<MessageType, IDictionary<string, string>> Parse(RedisLowLevelResponse response);

    }
}