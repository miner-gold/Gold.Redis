using System;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Models.Utils;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisKeysDb
    {
        /// <summary>
        /// Set key to hold the string value.
        /// If key already holds a value, it is overwritten, regardless of its type.
        /// Any previous time to live associated with the key is discarded on successful SET operation.
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The key that will be associated with the value</param>
        /// <param name="value">The value in the redis db of the key</param>
        /// <param name="expirySpan">Supported on Redis 2.6.12, The expiration span of the key</param>
        /// <param name="assertion">Supported on Redis 2.6.12, The assertion on key (Must Exists \ Must not exists)</param>
        /// <returns>True if the set operation was successful, False otherwise</returns>
        Task<bool> SetKey<T>(string key, T value, TimeSpan? expirySpan = null, KeyAssertion assertion = KeyAssertion.Any);

        /// <summary>
        /// Get the value of a key,
        /// If the key does not exists in the db, the default value of T will be returned
        /// </summary>
        /// <typeparam name="T">The expected Type of the returned value</typeparam>
        /// <param name="key">The redis key</param>
        /// <returns>The value of the key</returns>
        Task<T> Get<T>(string key);
    }
}
