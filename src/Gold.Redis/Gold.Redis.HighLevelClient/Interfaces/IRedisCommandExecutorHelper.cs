using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Models.Commands;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisCommandExecutorHelper
    {
        Task<bool> ExecuteBooleanIntResponseCommand(Command cmd);
        Task<int> ExecuteIntResponseCommand(Command cmd);

        Task<IEnumerable<T>> ExecuteArrayResponseCommand<T>(Command cmd);
    }
}
