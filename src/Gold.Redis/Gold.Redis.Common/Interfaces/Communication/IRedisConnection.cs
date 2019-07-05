using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common.Models;

namespace Gold.Redis.Common.Interfaces.Communication
{
    public interface IRedisConnection
    {
        Task<RedisLowLevelRespons> ExecuteCommand(string command);
    }
}
