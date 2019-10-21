using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Models.Commands;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisCommandExecutor
    {
        Task<T> Execute<T>(Command command) where T: Response;
    }
}
