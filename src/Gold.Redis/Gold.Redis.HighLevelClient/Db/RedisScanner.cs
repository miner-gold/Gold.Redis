using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<HashSet<string>> ExecuteFullScan(ScanCommandBase command)
        {
            var result = new HashSet<string>();
            int cursor = 0;
            do
            {
                command.Cursor = cursor;
                var response = await _commandExecutor.Execute<ArrayResponse>(command);
                if (response?.Responses == null)
                    return null;

                var responsesList = response.Responses.ToList();
                if (responsesList.Count < 2)
                    return null;

                cursor = (responsesList[0] as IntResponse).Response;
                foreach (var innerResponse in (responsesList[1] as ArrayResponse).Responses)
                {
                    result.Add((innerResponse as SimpleStringResponse).Response);
                }
            }
            while (cursor != 0);

            return result;
        }
    }
}
