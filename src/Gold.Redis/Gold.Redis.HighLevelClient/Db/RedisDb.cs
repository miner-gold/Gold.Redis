using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.HighLevelClient.Models.Utils;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisDb : IRedisDb
    {
        private readonly IRedisKeysDb _keysDb;
        private readonly IRedisSetDb _setDb;
        private readonly IRedisGeneralOperationsDb _generalOperationsDb;

        public RedisDb(
            IRedisGeneralOperationsDb generalOperationsDb,
            IRedisKeysDb keysDb,
            IRedisSetDb setDb)
        {
            _generalOperationsDb = generalOperationsDb;
            _keysDb = keysDb;
            _setDb = setDb;
        }

        #region General db operations
        public async Task<bool> IsKeyExists(string key) => await _generalOperationsDb.IsKeyExists(key);
        public async Task<bool> SetKeyExpire(string key, TimeSpan span) => await _generalOperationsDb.SetKeyExpire(key, span);
        public async Task<IEnumerable<string>> GetMatchingKeys(string pattern = "*") => await _generalOperationsDb.GetMatchingKeys(pattern);
        public async Task<bool> DeleteKey(string key) => await _generalOperationsDb.DeleteKey(key);
        public async Task<bool> FlushDb(bool isAsync = false) => await _generalOperationsDb.FlushDb(isAsync);
        public async Task<bool> Ping() => await _generalOperationsDb.Ping();
        #endregion

        #region Key operations
        public async Task<bool> SetKey<T>(string key, T value, TimeSpan? expirySpan = null, KeyAssertion assertion = KeyAssertion.Any) =>
            await _keysDb.SetKey(key, value, expirySpan, assertion);
        public async Task<T> Get<T>(string key) => await _keysDb.Get<T>(key);
        #endregion

        #region Sets operations

        public async Task<bool> SetAdd<T>(string key, T item) => await _setDb.SetAdd<T>(key, item);

        public async Task<bool> SetAddMultiple<T>(string key, IEnumerable<T> items) => await _setDb.SetAddMultiple(key, items);
        public async Task<int> SetCount(string key) => await _setDb.SetCount(key);
        public async Task<IEnumerable<T>> SetDiff<T>(string firstKey, params string[] keys) =>
            await _setDb.SetDiff<T>(firstKey, keys);
        public async Task<int> SetDiffStore(string resultingSetKey, string firstKey, params string[] keys) =>
            await _setDb.SetDiffStore(resultingSetKey, firstKey, keys);
        public async Task<IEnumerable<T>> SetIntersect<T>(params string[] keys) => await _setDb.SetIntersect<T>(keys);
        public async Task<int> SetIntersectStore(string resultingSetKey, params string[] keys) =>
            await _setDb.SetIntersectStore(resultingSetKey, keys);
        public async Task<bool> SetIsExists<T>(string key, T item) => await _setDb.SetIsExists<T>(key, item);
        public async Task<IEnumerable<T>> GetSetMembers<T>(string key) => await _setDb.GetSetMembers<T>(key);
        public async Task<bool> MoveItemBetweenSets<T>(string originSet, string destinationSet, T item) =>
            await _setDb.MoveItemBetweenSets<T>(originSet, destinationSet, item);
        public async Task<IEnumerable<T>> PopItemsFromSet<T>(string key, int numberOfItems) =>
            await _setDb.PopItemsFromSet<T>(key, numberOfItems);
        public async Task<T> PopItemFromSet<T>(string key) => await _setDb.PopItemFromSet<T>(key);
        public async Task<IEnumerable<T>> GetRandomItemsFromSet<T>(string key, int count, bool allowMultiple = false) =>
            await _setDb.GetRandomItemsFromSet<T>(key, count, allowMultiple);
        public async Task<T> GetRandomItemFromSet<T>(string key) => await _setDb.GetRandomItemFromSet<T>(key);
        public async Task<bool> SetRemove<T>(string key, params T[] itemsToRemove) => await _setDb.SetRemove<T>(key, itemsToRemove);
        public async Task<IEnumerable<T>> UnionSets<T>(params string[] keys) => await _setDb.UnionSets<T>(keys);
        public async Task<int> UnionSetsStore(string destinationKey, params string[] keys) =>
            await _setDb.UnionSetsStore(destinationKey, keys);
        public IAsyncEnumerable<T> SetScan<T>(string key, string pattern = null, int? countHint = null) =>  _setDb.SetScan<T>(key, pattern, countHint);
        #endregion
    }
}
