using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Communication
{
    public interface IRedisCommandHandler
    {
        Task<string> ExecuteCommand(string command);
    }
}
