using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisGeneralOperationsDb
    {
        Task<bool> IsKeyExists(string key);
        Task<bool> SetKeyExpire(string key, TimeSpan span);
        Task<IEnumerable<string>> GetMatchingKeys(string pattern = "*");
        Task<bool> DeleteKey(string key);

        /// <summary>
        /// Removes all the items from the db
        /// </summary>
        /// <param name="isAsync">Indicate if the operation should be preformed in async manner (require redis 4.0.0 or higher)</param>
        /// <returns></returns>
        Task<bool> FlushDb(bool isAsync = false);

        /// <summary>
        /// Tries to ping the redis db
        /// </summary>
        /// <returns>true if the response is PONG false otherwise</returns>
        Task<bool> Ping();
    }
}
