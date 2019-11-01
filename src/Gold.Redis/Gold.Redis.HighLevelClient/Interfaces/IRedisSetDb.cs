using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisSetDb
    {

        /// <summary>
        /// Same as SetAdd but with single item
        /// <param name="key">the identifier of the SET</param>
        /// </summary>
        Task<bool> SetAdd<T>(string key, T item);

        /// <summary>
        /// Add the specified members to the set stored at key.
        /// Specified members that are already a member of this set are ignored.
        /// If key does not exist, a new set is created before adding the specified members
        /// <param name="key">the identifier of the SET</param>
        /// </summary>
        Task<bool> SetAddMultiple<T>(string key, IEnumerable<T> items);

        /// <summary>
        /// Returns the number of elements of the set stored at key
        /// </summary>
        /// <param name="key">the identifier of the SET</param>
        /// <returns>Number of the set elements, 0 if the set does not exists</returns>
        Task<int> SetCount(string key);

        /// <summary>
        /// Returns the members of the set resulting from the difference between the first set and all the successive sets
        /// </summary>
        /// <typeparam name="T">The data type of the item in the set</typeparam>
        /// <param name="firstKey"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> SetDiff<T>(string firstKey, params string[] keys);

        /// <summary>
        /// This command is equal to SetDiff, but instead of returning the resulting set, it is stored in destination
        /// </summary>
        /// <param name="resultingSetKey">The new set with the DIFF operation result</param>
        /// <param name="firstKey"></param>
        /// <param name="keys"></param>
        /// <returns>The number of items that are in the result set</returns>
        Task<int> SetDiffStore(string resultingSetKey, string firstKey, params string[] keys);

        /// <summary>
        /// Returns the members of the set resulting from the intersection of all the given sets.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> SetIntersect<T>(params string[] keys);

        /// <summary>
        /// This command is equal to SetIntersect, but instead of returning the resulting set, it is stored in destination
        /// </summary>
        /// <param name="resultingSetKey">The new set with the INTERSECT operation result</param>
        /// <param name="keys"></param>
        /// <returns>The number of items that are in the result set</returns>
        Task<int> SetIntersectStore(string resultingSetKey, params string[] keys);

        /// <summary>
        /// Returns if the given item is in the set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> SetIsExists<T>(string key, T item);

        /// <summary>
        /// Gets all the items of a set from a given key
        /// </summary>
        /// <typeparam name="T">The data type of an item in the set</typeparam>
        /// <param name="key">The SET item key</param>
        /// <returns>A list of the items </returns>
        Task<IEnumerable<T>> GetSetMembers<T>(string key);

        /// <summary>
        /// Move member from the set at source to the set at destination.
        /// This operation is atomic. In every given moment the element will appear to be a member of source or destination for other clients
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originSet">The key of the set we want to take the item from</param>
        /// <param name="destinationSet">The key of the set we want to put the item in</param>
        /// <param name="item">The item we want to move between sets</param>
        /// <returns>True if the operation was successful, False otherwise</returns>
        Task<bool> MoveItemBetweenSets<T>(string originSet, string destinationSet, T item);

        /// <summary>
        /// Removes and returns number of random elements from the set value store at key
        /// </summary>
        /// <typeparam name="T">The set items data type</typeparam>
        /// <param name="key">The key of the set</param>
        /// <param name="numberOfItems">The number of items we want to take from the set</param>
        /// <returns></returns>
        Task<IEnumerable<T>> PopItemsFromSet<T>(string key, int numberOfItems);

        /// <summary>
        /// Same as PopItemsFromSet, but only pop one random item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> PopItemFromSet<T>(string key);

        /// <summary>
        /// Get number of random items from the SET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">the set key</param>
        /// <param name="count">the number of requested items to be returned</param>
        /// <param name="allowMultiple">If false, the command will only return distinct items. otherwise the command will return items even if it exits multiple times</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetRandomItemsFromSet<T>(string key, int count, bool allowMultiple = false);

        /// <summary>
        /// Return single random item from the set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetRandomItemFromSet<T>(string key);

        /// <summary>
        /// Removes an item from the set
        /// </summary>
        /// <typeparam name="T">The data type of an item in the set</typeparam>
        /// <param name="key">The SET item key</param>
        /// <param name="item"></param>
        /// <returns>True if the item was removed, false if the item does not exists nor the set does not exists</returns>
        Task<bool> SetRemove<T>(string key, params T[] itemsToRemove);

        /// <summary>
        /// Returns the members of the set resulting from the union of all the given sets
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> UnionSets<T>(params string[] keys);

        /// <summary>
        /// This command is equal to UnionSets, but instead of returning the resulting set, it is stored in destination
        /// </summary>
        /// <param name="destinationKey">The resulting SET key</param>
        /// <param name="keys"></param>
        /// <returns>The number of items that were added to the new set</returns>
        Task<int> UnionSetsStore(string destinationKey, params string[] keys);

        /// <summary>
        /// See https://redis.io/commands/scan for more information about the scan command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pattern"></param>
        /// <param name="countHint"></param>
        /// <returns>All of the item that match the given pattern in the set</returns>
        Task<IEnumerable<T>> SetScan<T>(string key, string pattern = null, int? countHint = null);

    }
}
