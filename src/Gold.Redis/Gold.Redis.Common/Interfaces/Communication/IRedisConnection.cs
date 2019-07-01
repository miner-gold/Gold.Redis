using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Communication
{
    public interface IRedisConnection
    {
        Task<string> ExecuteCommand(string command);
    }
}
