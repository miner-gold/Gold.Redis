using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.HighLevelClient.Models.Commands;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisCommandExecutorHelper : IRedisCommandExecutorHelper
    {
        private readonly IRedisCommandExecutor _executor;
        private readonly IStringResponseParser _parser;

        public RedisCommandExecutorHelper(IRedisCommandExecutor executor, IStringResponseParser parser)
        {
            _executor = executor;
            _parser = parser;
        }

        public async Task<bool> ExecuteBooleanIntResponseCommand(Command cmd)
        {
            return await ExecuteIntResponseCommand(cmd) == 1;
        }

        public async Task<int> ExecuteIntResponseCommand(Command cmd)
        {
            var response = await _executor.Execute<IntResponse>(cmd);
            return response.Response;
        }

        public async Task<IEnumerable<T>> ExecuteArrayResponseCommand<T>(Command cmd)
        {
            var response = await _executor.Execute<ArrayResponse>(cmd);
            return response.Responses.Select(res =>
            {
                var strResponse = res as BulkStringResponse;
                return _parser.Parse<T>(strResponse?.Response);
            });
        }
    }
}
