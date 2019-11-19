using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common.Interfaces.Pipelining;
using Gold.Redis.HighLevelClient.Interfaces;
using Gold.Redis.HighLevelClient.Models.Commands;
using Gold.Redis.HighLevelClient.Models.Utils;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.HighLevelClient.Db
{
    public class RedisWorkerCommandExecutor : IRedisCommandExecutor
    {
        private readonly IWorker<string, Response> _pipliningWorker;

        public RedisWorkerCommandExecutor(IWorker<string, Response> pipliningWorker)
        {
            _pipliningWorker = pipliningWorker;
        }

        public async Task<T> Execute<T>(Command command) where T : Response
        {
            var commandStr = command.GetCommandString();
            if (string.IsNullOrEmpty(commandStr))
            {
                throw new InvalidOperationException("Can not execute null command");
            }

            var work = await _pipliningWorker.Work(commandStr);
            return await work.WorkDone as T;
        }
    }
}
