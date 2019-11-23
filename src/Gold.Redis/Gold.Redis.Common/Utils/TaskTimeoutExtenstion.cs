using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gold.Redis.Common.Utils
{
    public static class TaskTimeoutExtenstion
    {
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            if (timeout == TimeSpan.Zero || timeout == TimeSpan.MaxValue)
                return await task;

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(
                    task,
                    Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;  // In order to propagate exceptions
                }

                throw new TimeoutException();
            }
        }

        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            if (timeout == TimeSpan.Zero || timeout == TimeSpan.MaxValue)
            {
                await task;
            }
            else
            {
                using (var timeoutCancellationTokenSource = new CancellationTokenSource())
                {
                    var completedTask = await Task.WhenAny(
                        task,
                        Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                    if (completedTask == task)
                    {
                        timeoutCancellationTokenSource.Cancel();
                        await task;  // In order to propagate exceptions
                    }
                    else
                    {
                        throw new TimeoutException();
                    }
                }
            }
        }
    }
}
