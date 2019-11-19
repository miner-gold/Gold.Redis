using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common.Interfaces.Pipelining;
using Gold.Redis.HighLevelClient.Models.Commands;
using Gold.Redis.HighLevelClient.Models.Utils;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Responses;
using NUnit.Framework;

namespace Gold.Redis.HighLevelClient.Pipeline
{
    public class PipelineExecutorWorkRunner : IWorkRunner<IList<string>, IList<Response>>
    {
        private readonly IRedisCommandHandler _pipelineCommandHandler;

        public PipelineExecutorWorkRunner(IRedisCommandHandler pipelineCommandHandler)
        {
            _pipelineCommandHandler = pipelineCommandHandler;
        }

        public async Task<IList<Response>> ExecuteWork(IList<string> commands)
        {
            var pipelineResponse = await _pipelineCommandHandler.ExecuteCommands<Response>(commands.ToArray());

            return pipelineResponse.ToArray();
        }
    }
}
