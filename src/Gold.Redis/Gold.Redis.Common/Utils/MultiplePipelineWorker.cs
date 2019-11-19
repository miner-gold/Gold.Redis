using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Gold.Redis.Common.Configuration;
using Gold.Redis.Common.Interfaces.Pipelining;

namespace Gold.Redis.Common.Utils
{
    public class MultiplePipelineWorker<T, U> : IWorker<T, U>
    {
        private BufferBlock<FutureWork<T, U>> _preBufferBlock;
        private BufferBlock<IList<FutureWork<T, U>>> _postBufferBlock;
        private ActionBlock<IList<FutureWork<T, U>>> _actionBlock;
        private readonly IWorkRunner<IList<T>, IList<U>> _runner;

        private readonly RedisPipelineConfiguration _pipelineConfiguration;


        public MultiplePipelineWorker(
            RedisPipelineConfiguration configuration,
            IWorkRunner<IList<T>, IList<U>> runner)
        {
            _pipelineConfiguration = configuration;
            _runner = runner;

            InitializeBufferBlocks();
        }

        private void InitializeBufferBlocks()
        {
            SetUpPreBufferBlock();
            SetUpPostBufferBlock();
        }

        private void SetUpPreBufferBlock()
        {
            _preBufferBlock = new BufferBlock<FutureWork<T, U>>(new DataflowBlockOptions
            {
                BoundedCapacity = _pipelineConfiguration.MaxItemsPerRequest
            });
        }

        private void SetUpPostBufferBlock()
        {
            _postBufferBlock = new BufferBlock<IList<FutureWork<T, U>>>();
            _actionBlock = new ActionBlock<IList<FutureWork<T, U>>>(
                async bufferAction => await RunActionOnItems(bufferAction),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = _pipelineConfiguration.MaxDegreeOfParallelism
                });
            _postBufferBlock.LinkTo(_actionBlock, new DataflowLinkOptions { PropagateCompletion = true });

            _preBufferBlock.AsObservable()
                .Buffer(_pipelineConfiguration.MaxWaitTime, _pipelineConfiguration.MaxItemsPerRequest)
                .Where(x => x.Any())
                .SelectMany(promiseWorks => Observable.FromAsync(
                    async () => await _postBufferBlock.SendAsync(promiseWorks)))
                .Subscribe();
        }

        private async Task RunActionOnItems(IList<FutureWork<T, U>> items)
        {
            try
            {
                var actionResults = await _runner.ExecuteWork(items.Select(futureWork => futureWork.Item).ToList());

                for (int i = 0; i < actionResults.Count; i++)
                {
                    items[i].Work.SetResult(actionResults[i]);
                }
            }
            catch (Exception ex)
            {
                foreach (var item in items)
                {
                    item.Work.SetException(ex);
                }
            }
        }

        public async Task<FutureWork<T, U>> Work(T input)
        {
            var promise = new FutureWork<T, U>(input);
            await _preBufferBlock.SendAsync(promise);
            return promise;
        }
    }
}
