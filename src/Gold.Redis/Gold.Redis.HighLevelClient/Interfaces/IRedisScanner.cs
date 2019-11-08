using System.Collections.Generic;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Models.Commands.Search;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisScanner
    {
        //TODO: on later versions (With .net standard 2.1 implement with IAsyncEnumerable)
        Task<HashSet<string>> ExecuteFullScan(ScanCommandBase command);
    }
}
