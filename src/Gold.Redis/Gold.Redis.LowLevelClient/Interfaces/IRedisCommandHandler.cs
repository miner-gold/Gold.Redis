using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface IRedisCommandHandler
    {
        Task<Response> ExecuteCommand(string command);
    }
}
