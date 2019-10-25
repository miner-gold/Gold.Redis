using System;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Models.Utils;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisKeysDb
    {
        Task<bool> SetKey<T>(string key, T value, TimeSpan? expirySpan = null, KeyAssertion assertion = KeyAssertion.Any);
        Task<T> Get<T>(string key);
    }
}
