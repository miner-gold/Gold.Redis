using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Utils;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisDbGeneralOperations
    {
        Task<bool> SetKey<T>(string key, T value, TimeSpan? expirySpan = null, KeyAssertion assertion = KeyAssertion.Any);
        Task<bool> IsKeyExists(string key);
        Task<T> Get<T>(string key);
        Task<bool> SetKeyExpire(string key, TimeSpan span);
        Task<IEnumerable<string>> GetMatchingKeys(string pattern = "*");
        Task<bool> DeleteKey(string key);

        /// <summary>
        /// Removes all the items from the db
        /// </summary>
        /// <param name="isAsync">Indicate if the operation should be preformed in async manner (require redis 4.0.0 or higher)</param>
        /// <returns></returns>
        Task<bool> FlushDb(bool isAsync = false);
    }
}
