using System;
using Gold.Redis.Common.Models;

namespace Gold.Redis.Common.Interfaces.Parsers
{
    public interface IMessageParser : IMessageResponseParser
    {
        bool Test(RedisLowLevelResponse response);

    }
}