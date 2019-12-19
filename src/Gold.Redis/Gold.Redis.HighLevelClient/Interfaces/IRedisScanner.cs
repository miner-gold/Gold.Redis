using System.Collections.Generic;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Models.Commands.Search;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisScanner
    {
        IAsyncEnumerable<string> ExecuteFullScan(ScanCommandBase command);
    }
}
