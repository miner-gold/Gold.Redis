using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.HighLevelClient.Models.Commands.Search;
using Gold.Redis.HighLevelClient.Models.Commands.Set;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisSetDb : IRedisSetDb
    {
        private readonly IRedisScanner _scanner;
        private readonly IRedisCommandExecutorHelper _commandExecutor;
        private readonly IStringResponseParser _parser;

        public RedisSetDb(
            IRedisCommandExecutorHelper commandExecutor,
            IStringResponseParser parser,
            IRedisScanner scanner)
        {
            _commandExecutor = commandExecutor;
            _parser = parser;
            _scanner = scanner;
        }

        public async Task<bool> SetAdd<T>(string key, T item) => await SetAddMultiple<T>(key, new List<T>() { item });

        public async Task<bool> SetAddMultiple<T>(string key, IEnumerable<T> items)
        {
            var itemsStr = items.Select(item => _parser.Stringify(item)).Distinct().ToList();
            var command = new SetAddCommand
            {
                SetKey = key,
                Items = itemsStr
            };

            var response = await _commandExecutor.ExecuteIntResponseCommand(command);
            return response == itemsStr.Count;
        }

        public async Task<int> SetCount(string key)
        {
            var command = new SetCardinalityCommand
            {
                SetKey = key
            };
            return await _commandExecutor.ExecuteIntResponseCommand(command);
        }

        public async Task<IEnumerable<T>> SetDiff<T>(string firstKey, params string[] keys)
        {
            var command = new SetDifferenceCommand
            {
                FirstSetKey = firstKey,
                DifferentSetKeys = keys
            };
            return await _commandExecutor.ExecuteArrayResponseCommand<T>(command);
        }

        public async Task<int> SetDiffStore(string resultingSetKey, string firstKey, params string[] keys)
        {
            var command = new SetDifferenceAndStoreCommand
            {
                NewSetKey = resultingSetKey,
                DifferentSetKeys = keys,
                FirstSetKey = firstKey
            };
            return await _commandExecutor.ExecuteIntResponseCommand(command);
        }

        public async Task<IEnumerable<T>> SetIntersect<T>(params string[] keys)
        {
            var command = new SetIntersectCommand
            {
                SetsKeys = keys
            };
            return await _commandExecutor.ExecuteArrayResponseCommand<T>(command);
        }

        public async Task<int> SetIntersectStore(string resultingSetKey, params string[] keys)
        {
            var command = new SetIntersectAndStoreCommand()
            {
                NewSetKey = resultingSetKey,
                SetsKeys = keys
            };
            return await _commandExecutor.ExecuteIntResponseCommand(command);
        }

        public async Task<bool> SetIsExists<T>(string key, T item)
        {
            var command = new SetIsMemberExistsCommand
            {
                SetKey = key,
                Member = _parser.Stringify(item)
            };
            return await _commandExecutor.ExecuteBooleanIntResponseCommand(command);
        }

        public async Task<IEnumerable<T>> GetSetMembers<T>(string key)
        {
            var command = new SetMembersCommand
            {
                SetKey = key
            };
            return await _commandExecutor.ExecuteArrayResponseCommand<T>(command);
        }

        public async Task<bool> MoveItemBetweenSets<T>(string originSet, string destinationSet, T item)
        {
            var command = new SetMoveCommand
            {
                SourceSet = originSet,
                DestinationSet = destinationSet,
                Item = _parser.Stringify(item)
            };
            return await _commandExecutor.ExecuteBooleanIntResponseCommand(command);
        }

        public async Task<IEnumerable<T>> PopItemsFromSet<T>(string key, int numberOfItems)
        {
            var command = new SetPopCommand
            {
                SetKey = key,
                NumberOfElements = numberOfItems
            };
            return await _commandExecutor.ExecuteArrayResponseCommand<T>(command);
        }

        public async Task<T> PopItemFromSet<T>(string key) => (await PopItemsFromSet<T>(key, 1)).FirstOrDefault();

        public Task<IEnumerable<T>> GetRandomItemsFromSet<T>(string key, int count, bool allowMultiple = false)
        {
            var command = new SetGetRandomMembersCommand
            {
                SetKey = key,
                NumberOfItems = count,
                AllowMultipleSameItemsReturn = allowMultiple
            };
            return _commandExecutor.ExecuteArrayResponseCommand<T>(command);
        }

        public async Task<T> GetRandomItemFromSet<T>(string key) =>
            (await GetRandomItemsFromSet<T>(key, 1)).FirstOrDefault();

        public async Task<bool> SetRemove<T>(string key, params T[] items)
        {
            var command = new SetRemoveCommand
            {
                SetKey = key,
                ItemsToRemove = items.Select(item => _parser.Stringify(item)).ToArray()
            };
            return await _commandExecutor.ExecuteIntResponseCommand(command) == items.Length;
        }

        public async Task<IEnumerable<T>> UnionSets<T>(params string[] keys)
        {
            var command = new SetUnionCommand
            {
                SetKeys = keys
            };
            return await _commandExecutor.ExecuteArrayResponseCommand<T>(command);
        }

        public async Task<int> UnionSetsStore(string destinationKey, params string[] keys)
        {
            var command = new SetUnionAndStoreCommand
            {
                NewSetKey = destinationKey,
                SetKeys = keys
            };
            return await _commandExecutor.ExecuteIntResponseCommand(command);
        }

        public async IAsyncEnumerable<T> SetScan<T>(string key, string pattern = null, int? countHint = null)
        {
            var command = new SetScanCommand
            {
                SetKey = key,
                CountHint = countHint,
                Match = pattern
            };
            var response = _scanner.ExecuteFullScan(command);
            await foreach (var i in response)
            {
                yield return _parser.Parse<T>(i);
            }
        }
    }
}
