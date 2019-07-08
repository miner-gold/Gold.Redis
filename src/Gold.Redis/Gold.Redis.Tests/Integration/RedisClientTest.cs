using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gold.Redis.Common;
using Gold.Redis.Common.Models.Configuration;
using Gold.Redis.LowLevelClient.Communication;
using Gold.Redis.LowLevelClient.Parsers;

namespace Gold.Redis.Tests.Integration
{
    public class RedisClientTest
    {
        protected RedisLowLevelClient CreateClient(RedisConnectionConfiguration connectionConfig)
        {

            var prefixParsers = new Dictionary<RedisResponseTypes, IPrefixParser>
            {
                {RedisResponseTypes.SimpleString, new SimpleStringParser()},
                {RedisResponseTypes.BulkString, new BulkStringParser()},
                {RedisResponseTypes.Integer, new IntegerParser()},
                {RedisResponseTypes.Error, new ErrorParser() }
            };
            var responseParser = new ResponseParser(prefixParsers
                .Concat(new[]
                    {new KeyValuePair<RedisResponseTypes, IPrefixParser>(RedisResponseTypes.Array, new ArrayParser(prefixParsers))})
                .ToDictionary(d => d.Key, d => d.Value));

            return new RedisLowLevelClient(
                new SocketsConnectionsContainer(connectionConfig), new RequestBuilder(),
                responseParser);
        }
    }
}
