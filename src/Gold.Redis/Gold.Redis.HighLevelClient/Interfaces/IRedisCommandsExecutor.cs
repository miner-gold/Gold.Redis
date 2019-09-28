using System.Threading.Tasks;
using Gold.Redis.Common.Models.Commands;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisCommandExecutor
    {
        Task<T> Execute<T>(Command command);
    }
}
