using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface IRedisCommandHandler
    {
        Task<T> ExecuteCommand<T>(string command) 
            where T : Response;
    }
}
