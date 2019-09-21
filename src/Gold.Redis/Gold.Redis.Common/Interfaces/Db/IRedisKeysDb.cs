using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Db
{
    public interface IRedisKeysDb : IRedisDbGeneralOperations
    {
        Task<bool> Set<T>(string key, T value);
    }
}
