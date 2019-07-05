using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common.Models;

namespace Gold.Redis.Common.Interfaces.Parsers
{
    public interface IResponseParser
    {
        Task<RedisLowLevelResponse> Parse(StreamReader stream);
    }
}
