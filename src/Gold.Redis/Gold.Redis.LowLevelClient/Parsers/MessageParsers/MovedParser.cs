using System.Collections.Generic;
using System.Transactions;
using Gold.Redis.Common;
using Gold.Redis.Common.Interfaces.Parsers;
using Gold.Redis.Common.Models;

namespace Gold.Redis.LowLevelClient.Parsers.MessageParsers
{
    public class MovedParser : IMessageParser
    {
        public KeyValuePair<MessageType, IDictionary<string, string>> Parse(RedisLowLevelResponse response)
        {
            var messageParts = response.Message.Split(' ');
            var connectionParts = messageParts[2].Split(':');
            var parameters = new Dictionary<string, string>
            {
                { Constants.HashMap, messageParts[1]},
                { Constants.Host, connectionParts[0]},
                { Constants.Port, connectionParts[1]}
            };
            return new KeyValuePair<MessageType, IDictionary<string, string>>(MessageType.Moved, parameters);
        }

        public bool Test(RedisLowLevelResponse response)
        {
            return response.ResponseType == RedisResponseTypes.Error &&
                   response.Message.StartsWith(Constants.MovedError);
        }
    }
}