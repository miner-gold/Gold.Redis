using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Models.Commands.Search;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisScanner
    {
        Task<HashSet<string>> ExecuteFullScan(ScanCommandBase command);
    }
}
