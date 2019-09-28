using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Interfaces
{
    public interface IRedisCommandHandler
    {
        Task<string> ExecuteCommand(string command);
    }
}
