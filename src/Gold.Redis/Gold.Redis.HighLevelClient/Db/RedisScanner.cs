using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.HighLevelClient.Models.Commands.Search;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisScanner : IRedisScanner
    {
        private readonly IRedisCommandExecutor _commandExecutor;

        public RedisScanner(IRedisCommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        public async IAsyncEnumerable<string> ExecuteFullScan(ScanCommandBase command)
        {
            int cursor = 0;
            do
            {
                command.Cursor = cursor;
                var response = await _commandExecutor.Execute<ArrayResponse>(command);
                if (response?.Responses == null)
                    yield break;

                var responsesList = response.Responses.ToList();
                if (responsesList.Count < 2)
                    yield break;

                cursor = int.Parse((responsesList[0] as BulkStringResponse).Response);
                foreach (var innerResponse in (responsesList[1] as ArrayResponse).Responses)
                {
                   yield return (innerResponse as BulkStringResponse).Response;
                }
            }
            while (cursor != 0);

        }
    }
}
