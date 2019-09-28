using System.Threading.Tasks;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisKeysDb : IRedisDbGeneralOperations
    {
        Task<bool> Set<T>(string key, T value);
    }
}
