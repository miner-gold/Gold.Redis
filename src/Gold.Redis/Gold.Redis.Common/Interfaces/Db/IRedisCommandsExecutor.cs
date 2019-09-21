using Gold.Redis.Common.Models.Commands;
using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Db
{
    public interface IRedisCommandExecutor
    {
        Task<T> Execute<T>(Command command);
    }
}
