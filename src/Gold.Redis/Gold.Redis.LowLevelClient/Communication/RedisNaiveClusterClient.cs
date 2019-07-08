using Gold.Redis.Common.Interfaces.Communication;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common.Interfaces.Parsers;
using Gold.Redis.Common.Models;
using Gold.Redis.Common.Models.Configuration;
using System.Collections.Generic;
using System.Linq;
using Gold.Redis.Common;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class RedisNaiveClusterClient : IRedisConnection
    {
        private IEnumerable<KeyValuePair<RedisConnectionConfiguration, IRedisConnection>> _clients;
        private readonly IMessageParser _parser;

        public RedisNaiveClusterClient(IMessageParser parser, 
            IEnumerable<KeyValuePair<RedisConnectionConfiguration, IRedisConnection>> clients)
        {
            _clients = clients;
            _parser = parser;
        }
        public async Task<RedisLowLevelResponse> ExecuteCommand(string command)
        {
            var result = await _clients.First().Value.ExecuteCommand(command);
            if (!_parser.Test(result))
                return result;
            var parsedMessage = _parser.Parse(result);
            var relevantClient = _clients.First(client => client.Key.Host == parsedMessage.Value[Constants.Host] &&
                                                          client.Key.Port == int.Parse(parsedMessage.Value[Constants.Port]));
            return await relevantClient.Value.ExecuteCommand(command);
        }


    }
}