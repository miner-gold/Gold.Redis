using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisDbGeneralOperations
    {
        Task<bool> IsKeyExists(string key);
        Task<T> Get<T>(string key);
        Task<bool> SetKeyExpire(string key, TimeSpan span);
        Task<IEnumerable<string>> GetMatchingKeys(string pattern);

    }
}
